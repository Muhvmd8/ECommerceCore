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
            var result = await identityService.CreateUserAsync(
                request.Email,
                request.Password,
                request.DisplayName,
                cancellationToken);

            if (result.IsFailure)
                return Result<RegisterResponse>.Failure(result.Error!);

            var newVerificationResult =
                await emailVerificationService.SendRegistrationCodeAsync(
                    result.Value.UserId,
                    result.Value.Email,
                    cancellationToken);

            if (newVerificationResult.IsFailure)
                return Result<RegisterResponse>.Failure(
                    newVerificationResult.Error!);

            return Result<RegisterResponse>.Success(
                new RegisterResponse(
                    result.Value.UserId.ToString(),
                    result.Value.Email));
        }
    }
}
