using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MulhacenLabs.Website.Services
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string ToAddress { get; set; } = string.Empty;
    }

    public interface IEmailService
    {
        Task SendContactEmailAsync(string senderName, string senderEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _settings = configuration.GetSection("Email").Get<EmailSettings>() ?? new EmailSettings();
            _logger = logger;
        }

        public async Task SendContactEmailAsync(string senderName, string senderEmail, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(_settings.Username) || string.IsNullOrWhiteSpace(_settings.ToAddress))
            {
                _logger.LogWarning("Email not sent: Email settings are not configured.");
                return;
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            email.To.Add(MailboxAddress.Parse(_settings.ToAddress));
            email.ReplyTo.Add(new MailboxAddress(senderName, senderEmail));
            email.Subject = $"[Contact] {subject}";
            email.Body = new TextPart("plain")
            {
                Text = $"From: {senderName} <{senderEmail}>\n\n{message}"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
