using FluentValidation;

namespace ECommerce.UseCases.Features.Auth.Commands.ResendEmailVerificationCode
{
    public sealed class ResendEmailVerificationCommandValidator
        : AbstractValidator<ResendEmailVerificationCommand>
    {
        public ResendEmailVerificationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);
        }
    }
}
