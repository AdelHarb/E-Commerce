namespace ECommerce.Dtos.AuthDtos;

public class AddRoleToUserDto
{
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Role { get; set; }
}