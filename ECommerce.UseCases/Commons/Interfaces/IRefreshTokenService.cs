using ECommerce.Domain.Shared;

namespace ECommerce.UseCases.Commons.Interfaces
{
    public sealed record RefreshTokenInfo(Guid UserId, string JwtId);
    public sealed record RefreshTokenResult(string Token, DateTime ExpiresAtUtc);
    public interface IRefreshTokenService
    {
        Task<Result<RefreshTokenResult>> GenerateAsync(
            Guid userId,
            string jwtId,
            CancellationToken cancellationToken = default);

        Task<Result<RefreshTokenInfo>> ValidateAsync(
            string token,
            CancellationToken cancellationToken = default);

        Task<Result> RevokeAsync(
            string token,
            CancellationToken cancellationToken = default);
    }
}
