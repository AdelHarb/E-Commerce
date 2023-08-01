using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Configurations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if(ModelState.IsValid is not true)
            return BadRequest(ModelState);
        
        var result = await _authService.RegisterAsync(registerDto);

        if(result.IsAuthenticated is not true)
            return BadRequest(result.Messages);
        
        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        
        return Ok(result);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDto logInDto)
    {
        if(ModelState.IsValid is not true)
            return BadRequest(ModelState);
        
        var result = await _authService.LoginAsync(logInDto);

        if(result.IsAuthenticated is not true)
            return BadRequest(result.Messages);
        
        if(!string.IsNullOrEmpty(result.RefreshToken))
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        
        return Ok(result);
    }
    
    [HttpPost("AddRoleToUser")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserDto addRoleToUserDto)
    {
        if(ModelState.IsValid is not true)
            return BadRequest(ModelState);
        
        var result = await _authService.AddRoleToUserAsync(addRoleToUserDto);

        if(string.IsNullOrEmpty(result) is not true)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var result = await _authService.RefreshTokenAsync(refreshToken);

        if(result.IsAuthenticated is not true)
            return BadRequest(result);
        
        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    
    [HttpPost("RevokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto revokeToken)
    {
        var token = revokeToken.Token ?? Request.Cookies["refreshToken"];
        
        if(string.IsNullOrEmpty(token) is not true)
            return BadRequest("Token is Required");
        
        var result = await _authService.RevokeTokenAsync(token);

        if(result is not true)
            return BadRequest("Invalid Token");
        
        return Ok();
    }
    
    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}