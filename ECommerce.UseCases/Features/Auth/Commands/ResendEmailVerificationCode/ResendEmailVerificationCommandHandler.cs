using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ResendEmailVerificationCode
{
    public sealed class ResendEmailVerificationCommandHandler(
        IIdentityService identityService,
        IEmailVerificationService emailVerificationService)
        : IRequestHandler<ResendEmailVerificationCommand, Result>
    {
        public async Task<Result> Handle(
            ResendEmailVerificationCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();

            var userResult = await identityService.GetUserByEmailAsync(
                email,
                cancellationToken);

            if (userResult.IsFailure)
                return Result.Failure(userResult.Error!);

            var user = userResult.Value;

            if (user.IsEmailConfirmed)
                return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

            return await emailVerificationService.SendRegistrationCodeAsync(
                user.UserId,
                user.Email,
                cancellationToken);
        }
    }
}
