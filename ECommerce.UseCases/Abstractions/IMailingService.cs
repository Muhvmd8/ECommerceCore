using Microsoft.AspNetCore.Http;

namespace ECommerce.UseCases.Abstractions
{
    public interface IMailingService
    {
        Task SendEmailAsync(
            string toEmail,
            string subject,
            string bodyHtml,
            IList<IFormFile>? attachments = null,
            CancellationToken cancellationToken = default);
    }
}
