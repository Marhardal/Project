using System.Net;
using System.Net.Mail;

namespace Project.Services
{
    public class NotificationService
    {
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config) => _config = config;

        public async Task SendMail(string toEmail, string subject, string body)
        {
            var host = _config["MailConfig:Host"]!;
            var port = int.Parse(_config["MailConfig:Port"]!);
            var senderEmail = _config["MailConfig:SenderEmail"]!;
            var senderName = _config["MailConfig:SenderName"]!;
            var password = _config["MailConfig:Password"]!;

            using var mail = new MailMessage();

            foreach (var email in toEmail.Split(',', StringSplitOptions.RemoveEmptyEntries))
                mail.To.Add(email.Trim());

            mail.From = new MailAddress(senderEmail, senderName);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Timeout = 30000   // ← 30 seconds, not 300000 (5 mins is too long)
            };

            try
            {
                await Task.Run(() => smtp.Send(mail));
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Email failed: {ex.Message}");
                throw;  // ← rethrow so the caller knows it failed
            }
        }
    }
}

