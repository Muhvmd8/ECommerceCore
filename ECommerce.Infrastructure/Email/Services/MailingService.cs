using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ECommerce.Infrastructure.Email.Services
{
    public sealed class MailingService(
        IOptions<SmtpSettings> options)
        : IMailingService
    {
        private readonly SmtpSettings _settings = options.Value;

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string bodyHtml,
            IList<IFormFile>? attachments = null,
            CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_settings.Username));

            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = bodyHtml
            };

            var logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                "logo.jpeg");

            if (File.Exists(logoPath))
            {
                var image = builder.LinkedResources.Add(logoPath);
                image.ContentId = "logo";
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls,
                cancellationToken);

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password,
                cancellationToken);

            await smtp.SendAsync(
                email,
                cancellationToken);

            await smtp.DisconnectAsync(
                true,
                cancellationToken);
        }
    }
}
