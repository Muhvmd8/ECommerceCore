using FluentValidation;

namespace ECommerce.UseCases.Features.Auth.Commands.VerifyRegistrationOtp
{
    public sealed class VerifyVerificationOtpCommandValidator
        : AbstractValidator<VerifyRegistrationOtpCommand>
    {
        public VerifyVerificationOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.Otp)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(6);
        }
    }
}