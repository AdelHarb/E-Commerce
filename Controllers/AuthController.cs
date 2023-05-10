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

        if(result.isAuthenticated is not true)
            return BadRequest(result.Messages);
        
        return Ok(result);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LogIn([FromBody] LoginDto logInDto)
    {
        if(ModelState.IsValid is not true)
            return BadRequest(ModelState);
        
        var result = await _authService.LoginAsync(logInDto);

        if(result.isAuthenticated is not true)
            return BadRequest(result.Messages);
        
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
}