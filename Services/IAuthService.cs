namespace ECommerce.Services;

public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterDto registerDto);
    Task<AuthModel> LoginAsync(LoginDto loginDto);
    Task<AuthModel> ForgetPassword(string email);
    Task<AuthModel> CheckCode(ConfirmationCodeDto confirmationCodeDto);
    Task<AuthModel> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<bool> SendEmail(MailRequestDto changePasswordDto);
    Task<string> AddRoleToUserAsync(AddRoleToUserDto addRoleToUserDto);
    Task<AuthModel> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
        
}