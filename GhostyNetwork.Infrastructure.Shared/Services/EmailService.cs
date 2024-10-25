using GhostyNetwork.Core.Application.Dtos.Email;
using GhostyNetwork.Core.Application.Interfaces.Services;
using GhostyNetwork.Core.Domain.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace GhostyNetwork.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private MailSettings _mailSettings { get;}

        public EmailService(IOptions<MailSettings> mailSetting)
        {
            _mailSettings = mailSetting.Value;
        }


        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} <{_mailSettings.EmailFrom}");
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                BodyBuilder builder = new();

                builder.HtmlBody = request.Body;

                email.Body = builder.ToMessageBody();

                // confi send mails

                using SmtpClient smtp = new();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error al enviar el email: {ex.Message}");
                return;
            }
        }
    }
}
