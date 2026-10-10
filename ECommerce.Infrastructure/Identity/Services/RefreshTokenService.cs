using ECommerce.Domain.Entities.Auth;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Infrastructure.Identity.Specifications;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Identity.Services
{
    public sealed class RefreshTokenService(
        IUnitOfWork unitOfWork,
        IOptions<JwtSettings> options)
        : IRefreshTokenService
    {
        private readonly JwtSettings _settings = options.Value;

        public async Task<Result<RefreshTokenResult>> GenerateAsync(
            Guid userId,
            string jwtId,
            CancellationToken cancellationToken = default)
        {
            var tokenValue = GenerateToken();

            var expiresAtUtc = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationInDays);
            var refreshToken = new RefreshToken
            {
                Token = tokenValue,
                ApplicationUserId = userId,
                JwtId = jwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAtUtc,
                IsUsed = false,
                IsRevoked = false
            };
            var repository = unitOfWork.GetRepository<RefreshToken>();
            await repository.AddAsync(refreshToken);
            await unitOfWork.SaveChangesAsync();

            return Result<RefreshTokenResult>.Success(
                new RefreshTokenResult(tokenValue, expiresAtUtc));
        }

        public async Task<Result<RefreshTokenInfo>> ValidateAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            var repository = unitOfWork.GetRepository<RefreshToken>();
            var refreshToken = await repository.FirstOrDefaultAsync(new GetRefreshTokenSpecification(token));

            if (refreshToken is null)
                return Result<RefreshTokenInfo>.Failure(IdentityErrors.InvalidRefreshToken);

            if (refreshToken.IsRevoked ||
                refreshToken.IsUsed ||
                refreshToken.ExpiresAt <= DateTime.UtcNow)
                return Result<RefreshTokenInfo>.Failure(IdentityErrors.InvalidRefreshToken);

            return Result<RefreshTokenInfo>.Success(
                new RefreshTokenInfo(
                    refreshToken.ApplicationUserId,
                    refreshToken.JwtId));
        }

        public async Task<Result> RevokeAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            var repository = unitOfWork.GetRepository<RefreshToken>();
            var refreshToken = await repository.FirstOrDefaultAsync(new GetRefreshTokenSpecification(token));

            if (refreshToken is null)
                return Result.Failure(IdentityErrors.InvalidRefreshToken);

            refreshToken.IsRevoked = true;
            refreshToken.IsUsed = true;

            repository.Update(refreshToken);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        private static string GenerateToken()
        {
            Span<byte> bytes = stackalloc byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    }
}
