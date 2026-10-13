using ECommerce.Domain.Shared;
using ECommerce.UseCases.Commons.DTOs;

namespace ECommerce.UseCases.Abstractions
{
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
