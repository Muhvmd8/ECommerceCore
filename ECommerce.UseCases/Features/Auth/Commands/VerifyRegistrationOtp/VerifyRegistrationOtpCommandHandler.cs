using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.VerifyRegistrationOtp
{
    public sealed class VerifyRegistrationOtpCommandHandler(IEmailVerificationService verificationCodeService)
        : IRequestHandler<VerifyRegistrationOtpCommand, Result>
    {
        public async Task<Result> Handle(VerifyRegistrationOtpCommand request, CancellationToken cancellationToken)
        {
            var result = await verificationCodeService.VerifyRegistrationCodeAsync(
                request.Email,
                request.Otp,
                cancellationToken);

            if (result.IsFailure)
                return Result.Failure(result.Error!);

            return Result.Success();
        }
    }
}