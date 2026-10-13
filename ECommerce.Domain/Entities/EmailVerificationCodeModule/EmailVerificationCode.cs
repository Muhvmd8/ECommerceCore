namespace ECommerce.Domain.Entities.EmailVerificationCodeModule
{
    public sealed class EmailVerificationCode
    {
        public Guid UserId { get; init; }

        public string Code { get; init; } = string.Empty;

        public EmailVerificationPurpose Purpose { get; init; }

        public DateTimeOffset ExpirationTimeUtc { get; init; }
    }
}
