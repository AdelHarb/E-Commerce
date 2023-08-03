using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using ECommerce.Services.Email;

namespace ECommerce.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMailingService _mailingService;
    private readonly IMapper _mapper;
    private readonly JWT _jwt;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IOptions<JWT> jwt,
        RoleManager<IdentityRole> roleManager,
        IMailingService mailingService
    )
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwt = jwt.Value;
        _roleManager = roleManager;
        _mailingService = mailingService;
    }

    public async Task<string> AddRoleToUserAsync(AddRoleToUserDto addRoleToUserDto)
    {
        var user = await _userManager.FindByIdAsync(addRoleToUserDto.UserId);

        if (user is null)
            return "User not found";

        if (await _roleManager.RoleExistsAsync(addRoleToUserDto.Role) is not true)
            return "Role not found";

        if (await _userManager.IsInRoleAsync(user, addRoleToUserDto.Role))
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

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(await CreateJwtToken(user));
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
        authModel.ExpiresOn = DateTime.Now.AddDays(_jwt.DurationInDays);

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.Where(t => t.IsActive).FirstOrDefault();
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
        }

        return authModel;
    }

    public async Task<AuthModel> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            return new AuthModel { Messages = "Email already registered" };

        if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            return new AuthModel { Messages = "Username already registered" };

        var user = _mapper.Map<ApplicationUser>(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded is not true)
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
            IsAuthenticated = true,
            Roles = new List<string> { Roles.User },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Username = user.UserName,
            RefreshToken = GenerateRefreshToken().Token,
            RefreshTokenExpiration = DateTime.Now.AddDays(7),
        };
    }

    public async Task<AuthModel> ForgetPassword(string email)
    {
        var authModel = new AuthModel();
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if(string.IsNullOrEmpty(email))
        {
            authModel.Messages = "Email is Invalid";
            return authModel;
        }
        
        if(user is null)
        {
            authModel.Messages = "Email Does not Exist";
            return authModel;
        }
        user.Code = GenerateVerificationCode();

        await _mailingService.SendEmailAsync(email, "Reset Password", $"Your Reset Code is {user.Code}");
        authModel.Messages = "Reset Code Sent to your Email";
        return authModel;
    }
    
    public async Task<AuthModel> CheckCode(ConfirmationCodeDto confirmationCodeDto)
    {
        var authModel = new AuthModel();
        ApplicationUser? user = await _userManager.FindByEmailAsync(confirmationCodeDto.Email);

        if(user is null)
        {
            authModel.Messages = "Email Does not Exist";
            return authModel;
        }

        if(user.Code != confirmationCodeDto.Code)
        {
            authModel.Messages = "Invalid Code";
            return authModel;
        }

        authModel.Messages = "Code Verified";
        return authModel;
    }

    public async Task<AuthModel> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var authModel = new AuthModel();

        if(string.IsNullOrEmpty(resetPasswordDto.Email))
        {
            authModel.Messages = "Invalid Email";
            return authModel;
        }
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        
        if(user is null)
        {
            authModel.Messages = "Email does not Exist";
            return authModel;
        }
        if(resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
        {
            authModel.Messages = "Confirmed Password Doest not match the Password";
            return authModel;
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ChangePasswordAsync(user, token,resetPasswordDto.NewPassword);

        if(!result.Succeeded)
        {
            authModel.Messages = "Failed to reset Password";
            return authModel;
        }

        authModel.Messages = "Password Has Been Rest";
        return authModel;


    }
    public async Task<bool> SendEmail (MailRequestDto mailRequestDto)
    {
       var result = await _mailingService.SendEmailAsync(mailRequestDto.Email, mailRequestDto.Subject, mailRequestDto.Body);
         return result;
    }
    
    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();

        var user = await _userManager.Users.SingleOrDefaultAsync(
            u => u.RefreshTokens.Any(t => t.Token == token)
        );

        if (user == null)
        {
            authModel.Messages = "Invalid Token";
            return authModel;
        }
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        if (!refreshToken.IsActive)
        {
            authModel.Messages = "Inactive Token";
            return authModel;
        }
        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
        authModel.ExpiresOn = DateTime.Now.AddDays(_jwt.DurationInDays);
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return authModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(
            u => u.RefreshTokens.Any(t => t.Token == token)
        );

        if (user is null)
            return false;

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            return false;

        refreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return true;
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
        var signingCredentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256
        );

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomRandom = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomRandom);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomRandom),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };
    }

    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(10000, 99999).ToString();
    }

}
