using ECommerce.Domain.Entities.EmailVerificationCodeModule;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;

namespace ECommerce.Infrastructure.Repositories
{
    public sealed class EmailStore(
        ICachedAggregateStore<EmailVerificationCode> store)
        : IEmailStore
    {
        private const string KeyPrefix = "otp:email:";

        public async Task SaveAsync(
            EmailVerificationCode verificationCode,
            CancellationToken cancellationToken = default)
        {
            var key = _BuildKey(verificationCode.UserId, verificationCode.Purpose);

            await store.RemoveAsync(key, cancellationToken);

            await store.GetOrCreateAsync(
                key,
                _ => Task.FromResult(verificationCode),
                cancellationToken);
        }

        public async Task<EmailVerificationCode?> GetAsync(
            Guid userId,
            EmailVerificationPurpose purpose,
            CancellationToken cancellationToken = default)
        {
            var key = _BuildKey(userId, purpose);

            var verificationCode = await store.GetAsync(
                key,
                cancellationToken);

            if (verificationCode is null)
                return null;

            if (verificationCode.ExpirationTimeUtc <=
                DateTimeOffset.UtcNow)
            {
                await store.RemoveAsync(key, cancellationToken);
                return null;
            }

            return verificationCode;
        }

        public async Task RemoveAsync(
            Guid userId,
            EmailVerificationPurpose purpose,
            CancellationToken cancellationToken = default)
        {
            await store.RemoveAsync(
                _BuildKey(userId, purpose),
                cancellationToken);
        }

        private static string _BuildKey(
            Guid userId,
            EmailVerificationPurpose purpose)
            => $"{KeyPrefix}{purpose}:{userId}";
    }
}
