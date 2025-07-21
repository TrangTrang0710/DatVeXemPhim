using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Nhom7_DoAn_DangKy_DangNhap.Services
{
    public class EmailSettings
    {
        public string? SenderEmail { get; set; }
        public string? SenderPassword { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
    }

    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_settings.SmtpServer, _settings.Port))
            {
                client.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
        }
    }
}
