using FluentValidation;

namespace ECommerce.UseCases.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandValidator
        : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
