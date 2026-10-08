using FluentValidation;

namespace ECommerce.UseCases.Features.Products.Queries.GetAllPaginatedProducts
{
    internal class GetAllPaginatedQueryValidator : AbstractValidator<GetAllPaginatedProductsQuery>
    {
        public GetAllPaginatedQueryValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithErrorCode("Products.PageNumber.Invalid")
                .WithMessage("Page Number must be at least 1");

            RuleFor(p => p.PageSize)
                .ExclusiveBetween(1, 100)
                .WithErrorCode("Products.PageSize.Invalid")
                .WithMessage("Page Size must be between 1 and 100");
        }
    }
}