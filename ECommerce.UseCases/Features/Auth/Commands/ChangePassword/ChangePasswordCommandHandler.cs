using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandHandler(
        IIdentityService identityService)
        : IRequestHandler<ChangePasswordCommand, Result>
    {
        public async Task<Result> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            return await identityService.ChangePasswordAsync(
                request.UserId,
                request.CurrentPassword,
                request.NewPassword,
                cancellationToken);
        }
    }
}
