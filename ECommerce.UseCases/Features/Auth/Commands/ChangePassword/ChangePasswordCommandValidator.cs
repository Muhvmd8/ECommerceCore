using FluentValidation;

namespace ECommerce.UseCases.Features.Auth.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandValidator
        : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from current password.");
        }
    }
}
