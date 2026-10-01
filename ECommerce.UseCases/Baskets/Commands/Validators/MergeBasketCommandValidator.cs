using FluentValidation;

namespace ECommerce.UseCases.Baskets.Commands.Validators
{
    public sealed class MergeBasketCommandValidator
        : AbstractValidator<MergeBasketCommand>
    {
        public MergeBasketCommandValidator()
        {
            RuleFor(x => x.BuyerId)
                .NotEmpty()
                .WithMessage("Buyer ID is required.");

            RuleFor(x => x.AnonymousBuyerId)
                .NotEmpty()
                .WithMessage("Anonymous buyer ID is required.");

            RuleFor(x => x)
                .Must(x => x.BuyerId != x.AnonymousBuyerId)
                .WithMessage("Buyer ID and anonymous buyer ID cannot be the same.");
        }
    }
}
