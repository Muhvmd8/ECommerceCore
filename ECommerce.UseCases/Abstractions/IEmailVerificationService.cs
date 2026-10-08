using ECommerce.Domain.Shared;
namespace ECommerce.UseCases.Abstractions
{
    public interface IEmailVerificationService
    {
        Task<Result> SendRegistrationCodeAsync(
            Guid userId,
            string email,
            CancellationToken cancellationToken = default);

        Task<Result> VerifyRegistrationCodeAsync(
            string email,
            string code,
            CancellationToken cancellationToken = default);

        Task<Result> SendPasswordResetCodeAsync(
            Guid userId,
            string email,
            CancellationToken cancellationToken = default);

        Task<Result<string>> VerifyPasswordResetCodeAsync(
            Guid userId,
            string code,
            CancellationToken cancellationToken = default);
    }
}
