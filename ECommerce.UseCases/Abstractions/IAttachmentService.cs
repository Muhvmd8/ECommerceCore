using ECommerce.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ECommerce.UseCases.Abstractions
{
    public interface IAttachmentService
    {
        Task<Result<string>> UploadAttachmentAsync(IFormFile file);
    }
}
