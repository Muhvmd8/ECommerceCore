using ECommerce.Domain.Entities.EmailVerificationCodeModule;

namespace ECommerce.Domain.Repositories
{
    public interface IEmailStore
    {
        Task SaveAsync(
            EmailVerificationCode verificationCode,
            CancellationToken cancellationToken = default);

        Task<EmailVerificationCode?> GetAsync(
            Guid userId,
            EmailVerificationPurpose purpose,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            Guid userId,
            EmailVerificationPurpose purpose,
            CancellationToken cancellationToken = default);
    }
}
