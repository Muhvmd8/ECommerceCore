using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ChangePassword
{
    public sealed record ChangePasswordCommand(
        Guid UserId,
        string CurrentPassword,
        string NewPassword)
        : IRequest<Result>;
}
