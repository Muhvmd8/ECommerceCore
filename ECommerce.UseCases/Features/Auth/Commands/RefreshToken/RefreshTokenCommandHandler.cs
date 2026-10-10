using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IIdentityService identityService,
        IJwtTokenGenerator jwtService)
        : IRequestHandler<
            RefreshTokenCommand,
            Result<RefreshTokenResponse>>
    {
        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await refreshTokenService.ValidateAsync(
                    request.RefreshToken,
                    cancellationToken);

            if (validationResult.IsFailure)
                return Result<RefreshTokenResponse>.Failure(
                    validationResult.Error!);

            var tokenInfo = validationResult.Value!;

            // هنا تجيب User + Roles
            var userResult = await identityService.GetUserByIdAsync(tokenInfo.UserId, cancellationToken);
            var userRoles = (await identityService.GetRolesAsync(tokenInfo.UserId, cancellationToken)).Value;

            if (userResult.IsFailure)
                return Result<RefreshTokenResponse>.Failure(userResult.Error!);

            var jwtResult = jwtService.GenerateToken(
                userResult.Value.UserId,
                userResult.Value.Email,
                userResult.Value.DisplayName,
                userRoles);

            var newRefreshTokenResult = await refreshTokenService.GenerateAsync(
                tokenInfo.UserId,
                jwtResult.JwtId,
                cancellationToken);

            if (newRefreshTokenResult.IsFailure)
                return Result<RefreshTokenResponse>.Failure(newRefreshTokenResult.Error!);

            var revokeResult = await refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken);

            if (revokeResult.IsFailure)
                return Result<RefreshTokenResponse>.Failure(revokeResult.Error!);

            return Result<RefreshTokenResponse>.Success(
                new RefreshTokenResponse(
                    jwtResult.Token,
                    newRefreshTokenResult.Value.Token,
                    jwtResult.ExpiresAtUtc,
                    newRefreshTokenResult.Value.ExpiresAtUtc));
        }
    }
}
