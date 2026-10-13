using ECommerce.Domain.Entities.Auth;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using ECommerce.UseCases.Commons.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Identity.Services
{
    public sealed class RefreshTokenService(
        ApplicationIdentityDbContext context,
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
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return Result<RefreshTokenResult>.Success(
                new RefreshTokenResult(tokenValue, expiresAtUtc));
        }

        public async Task<Result<RefreshTokenInfo>> ValidateAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            var refreshToken = await context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);

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
            var refreshToken = await context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);

            if (refreshToken is null)
                return Result.Failure(IdentityErrors.InvalidRefreshToken);

            refreshToken.IsRevoked = true;
            refreshToken.IsUsed = true;

            context.RefreshTokens.Update(refreshToken);
            await context.SaveChangesAsync(cancellationToken);

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
