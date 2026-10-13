using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Login
{

    public sealed class LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenService refreshTokenService,
        INotificationRealTimeSender notificationRealTimeSender)
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var credentialsResult =
                await identityService.ValidateCredentialsAsync(
                    request.Email.Trim(),
                    request.Password,
                    cancellationToken);

            if (credentialsResult.IsFailure)
            {
                return Result<LoginResponse>.Failure(credentialsResult.Error!);
            }

            var user = credentialsResult.Value;

            var rolesResult = await identityService.GetRolesAsync(
                user.UserId,
                cancellationToken);

            if (rolesResult.IsFailure)
            {
                return Result<LoginResponse>.Failure(
                    rolesResult.Error!);
            }

            var accessToken = jwtTokenGenerator.GenerateToken(
                user.UserId,
                user.Email,
                user.DisplayName,
                rolesResult.Value);

            var refreshTokenResult =
                await refreshTokenService.GenerateAsync(
                    user.UserId,
                    accessToken.JwtId,
                    cancellationToken);

            if (refreshTokenResult.IsFailure)
            {
                return Result<LoginResponse>.Failure(
                    refreshTokenResult.Error!);
            }

            var refreshToken = refreshTokenResult.Value;

            await notificationRealTimeSender.SendToUserAsync(
                user.UserId,
                new
                {
                    user.UserId,
                    user.Email,
                    user.DisplayName
                },
                cancellationToken);

            return Result<LoginResponse>.Success(
                new LoginResponse(
                    accessToken.Token,
                    refreshToken.Token,
                    accessToken.ExpiresAtUtc,
                    refreshToken.ExpiresAtUtc));
        }
    }

}
