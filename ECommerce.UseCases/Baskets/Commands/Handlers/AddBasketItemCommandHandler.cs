using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Products;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.Handlers
{
    public sealed class AddBasketItemCommandHandler(IBasketRepository basketRepository,
        IReadRepository<Product> productRepo)
        : IRequestHandler<AddBasketItemCommand, Result<GetBasketResponse>>
    {
        public async Task<Result<GetBasketResponse>> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepo.FirstOrDefaultAsync(
                new ProductForBasketSpecification(request.ProductId), cancellationToken);

            if (product is null)
                return Result<GetBasketResponse>.Failure(ProductErrors.ProductNotFound);

            var basket = await basketRepository.GetOrCreateAsync(request.BuyerId, cancellationToken);

            var addItemResult = basket.AddItem(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price,
                request.Quantity);

            if (addItemResult.IsFailure)
                return Result<GetBasketResponse>.Failure(addItemResult.Error!);

            await basketRepository.SaveAsync(basket, cancellationToken);
            return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
        }
    }
}
