using Eko.Database.Entities;
using MailKit.Security;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Eko.Common.EmailService;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendApplicationConfirmationAsync(Notification notification)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Fayzullin Group Company", _emailSettings.SenderEmail));
        message.To.Add(new MailboxAddress(notification.Name, notification.Email));
        message.Subject = notification.Subject;
        var body = new BodyBuilder
        {
            TextBody =
                $"Hello,{notification.Name} your application has been accepted, thank you for sending, our manager will contact you soon to clarify the details.\nGoodbye"
        };
        message.Body = body.ToMessageBody();
        
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

public interface IEmailService
{
    Task SendApplicationConfirmationAsync(Notification notification);
}