using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string RefreshToken)
        : IRequest<Result<RefreshTokenResponse>>;
}
