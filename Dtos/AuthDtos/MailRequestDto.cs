namespace ECommerce.Dtos.AuthDtos;

public class MailRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}