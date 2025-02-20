using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;
using System.Net.Mail;

namespace AvaluoAPI.Application.Handlers
{
    // EmailService.cs
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, IFormFile attachment);
    }

    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["Email:FromEmail"]!;
            _password = configuration["Email:Password"]!;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, IFormFile attachment)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body
            };
            message.To.Add(toEmail);

            if (attachment != null)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                ms.Position = 0;
                message.Attachments.Add(new System.Net.Mail.Attachment(ms, attachment.FileName));
            }

            using var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_fromEmail, _password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
