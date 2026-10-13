using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Register
{

    public sealed class RegisterCommandHandler(
        IIdentityService identityService,
        IEmailVerificationService emailVerificationService)
        : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        public async Task<Result<RegisterResponse>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();

            var createResult = await identityService.CreateUserAsync(
                email,
                request.Password,
                request.DisplayName,
                cancellationToken);

            if (createResult.IsFailure)
            {
                return Result<RegisterResponse>.Failure(
                    createResult.Error!);
            }

            var user = createResult.Value;

            var verificationResult =
                await emailVerificationService.SendRegistrationCodeAsync(
                    user.UserId,
                    user.Email,
                    cancellationToken);

            if (verificationResult.IsFailure)
            {
                return Result<RegisterResponse>.Failure(
                    verificationResult.Error!);
            }

            return Result<RegisterResponse>.Success(
                new RegisterResponse(
                    user.UserId.ToString(),
                    user.Email));
        }
    }

}
