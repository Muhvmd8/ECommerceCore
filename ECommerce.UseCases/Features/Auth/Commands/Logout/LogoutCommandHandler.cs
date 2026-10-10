using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public LogoutCommandHandler(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    public async Task<Result> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return Result.Failure(IdentityErrors.InvalidRefreshToken);

        return await _refreshTokenService.RevokeAsync(
            request.RefreshToken.Trim(),
            cancellationToken);
    }
}
