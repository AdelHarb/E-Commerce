namespace ECommerce.Dtos.AuthDtos;

public class RegisterDto
{
    [Required, StringLength(20)]
    public string FirstName {get; set;}
    [Required, StringLength(20)]
    public string LastName {get; set;}
    [Required, StringLength(20)]
    public string UserName {get; set;}
    [Required, StringLength(128)]
    public string Email {get; set;}
    [Required, StringLength(20)]
    public string Address {get; set;}
    [Required, StringLength(32)]
    public string Password {get; set;}

}