using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ECommerce.Services.Email;

public class MailingService : IMailingService
{
    private readonly MailSettings _mailSettings;
    public MailingService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    public async Task<bool> SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null!)
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Email),
            Subject = subject
        };
        email.To.Add(MailboxAddress.Parse(mailTo));

        var builder = new BodyBuilder();

        if(attachments is not null)
        {
            byte[] fileBytes;
            foreach (var attachment in attachments)
            {
                if (attachment.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await attachment.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                    
                    builder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
        }

        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);

        await smtp.SendAsync(email);

        smtp.Disconnect(true);

        return true;
    }
}