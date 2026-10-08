using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Register
{
    public sealed record RegisterCommand(
        string Email,
        string Password,
        string DisplayName)
        : IRequest<Result<RegisterResponse>>;
}