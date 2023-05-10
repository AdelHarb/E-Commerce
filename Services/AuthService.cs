using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly JWT _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JWT> jwt,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwt = jwt.Value;
        _roleManager = roleManager;
    }

    public async Task<string> AddRoleToUserAsync(AddRoleToUserDto addRoleToUserDto)
    {
        var user = await _userManager.FindByIdAsync(addRoleToUserDto.UserId);

        if(user is null)
            return "User not found";
        
        if(await _roleManager.RoleExistsAsync(addRoleToUserDto.Role) is not true)
            return "Role not found";
        
        if(await _userManager.IsInRoleAsync(user, addRoleToUserDto.Role))
            return "User already has this role";
        
        var result = await _userManager.AddToRoleAsync(user, addRoleToUserDto.Role);

        return result.Succeeded ? string.Empty : "Something went wrong";    

    }

    public async Task<AuthModel> LoginAsync(LoginDto loginDto)
    {
        var authModel = new AuthModel();

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            authModel.Messages = "Invalid Credentials";
            return authModel;
        }

        authModel.isAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(await CreateJwtToken(user));
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
        authModel.ExpiresOn = DateTime.Now.AddDays(_jwt.DurationInDays);
        
        return authModel;
    }

    public async Task<AuthModel> RegisterAsync(RegisterDto registerDto)
    {
        if(await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            return new AuthModel { Messages = "Email already registered" };
        
        if(await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            return new AuthModel { Messages = "Username already registered" };
        
        var user = _mapper.Map<ApplicationUser>(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if(result.Succeeded is not true)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
            {
                errors += $"{error.Description},";
            }
            return new AuthModel { Messages = errors };
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        var jwtSecurityToken = await CreateJwtToken(user);
        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            isAuthenticated = true,
            Roles = new List<string> { Roles.User },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName
        };
    }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}