using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SpendingsApi.IServices;
using SpendingsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingsApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string CreateHTMLTableAsync(List<Spendings> spendings)
        {
            return "";
        }
        public void Send(Email email)
        {
            // tworzenie wiadomości email
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(email.From));
            emailToSend.To.Add(MailboxAddress.Parse(email.To));
            emailToSend.Subject = email.Subject;
            emailToSend.Body = new TextPart(TextFormat.Html) { Text = email.Html };

            // wysyłanie wiadomości email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            smtp.Send(emailToSend);
            smtp.Disconnect(true);
        }
    }
}
