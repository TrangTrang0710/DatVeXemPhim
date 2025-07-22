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

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(_settings.SenderEmail))
                throw new Exception("❌ Lỗi: 'SenderEmail' bị null hoặc rỗng. Vui lòng kiểm tra cấu hình trong appsettings.json");
            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(_settings.SenderEmail, toEmail, subject, body);
            await smtp.SendMailAsync(mail);
        }
    }
}
