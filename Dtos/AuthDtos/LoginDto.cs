namespace ECommerce.Dtos.AuthDtos;

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}