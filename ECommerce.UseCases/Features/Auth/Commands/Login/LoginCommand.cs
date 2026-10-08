using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password)
        : IRequest<Result<LoginResponse>>;
}