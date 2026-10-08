using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Interfaces;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Login
{
    public sealed class LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenService refreshTokenService)
        : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.ValidateCredentialsAsync(
                request.Email,
                request.Password,
                cancellationToken);

            if (result.IsFailure)
                return Result<LoginResponse>.Failure(result.Error!);

            var userRolesResult = await identityService.GetRolesAsync(result.Value.UserId, cancellationToken);
            if (userRolesResult.IsFailure)
                return Result<LoginResponse>.Failure(userRolesResult.Error!);

            var accessTokenResult = jwtTokenGenerator.GenerateToken(result.Value.UserId, result.Value.Email, result.Value.DisplayName, userRolesResult.Value);
            var refreshTokenResult = await refreshTokenService.GenerateAsync(result.Value.UserId, accessTokenResult.JwtId, cancellationToken);
            if (refreshTokenResult.IsFailure)
                return Result<LoginResponse>.Failure(refreshTokenResult.Error!);

            var loginResponse = new LoginResponse(
                accessTokenResult.Token,
                refreshTokenResult.Value.Token,
                accessTokenResult.ExpiresAtUtc,
                refreshTokenResult.Value.ExpiresAtUtc);

            return Result<LoginResponse>.Success(loginResponse);
        }
    }
}
