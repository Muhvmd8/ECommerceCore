namespace ECommerce.UseCases.Abstractions
{
    public interface IIpSecurityService
    {
        Task<bool> TooManyFailedLogins(
            string ip,
            int maxAttempts,
            TimeSpan window,
            CancellationToken cancellationToken = default);
        Task BlockIpAsync(
            string ip,
            string reason,
            TimeSpan duration,
            CancellationToken cancellationToken = default);
        Task<bool> IsIpBlockedAsync(string ip, CancellationToken cancellationToken = default);
        bool ContainsMaliciousPayload(string? body);
    }
}
