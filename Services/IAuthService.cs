namespace ECommerce.Services;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterDto registerDto);
    Task<AuthModel> LoginAsync(LoginDto loginDto);
    Task<string> AddRoleToUserAsync(AddRoleToUserDto addRoleToUserDto);
}