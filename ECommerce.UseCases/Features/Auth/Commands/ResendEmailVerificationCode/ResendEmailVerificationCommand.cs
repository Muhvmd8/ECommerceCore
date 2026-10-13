using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ResendEmailVerificationCode
{
    public sealed record ResendEmailVerificationCommand(
        string Email) : IRequest<Result>;
}
