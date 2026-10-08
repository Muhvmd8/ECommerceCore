using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Products;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.UpdateBasketItemQuantity
{
    public sealed class UpdateBasketItemQuantityCommandHandler(IBasketRepository basketRepository,
        IReadRepository<Product> productRepo)
        : IRequestHandler<UpdateBasketItemQuantityCommand, Result<GetBasketResponse>>
    {
        public async Task<Result<GetBasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepo.FirstOrDefaultAsync(
            new ProductForBasketSpecification(request.ProductId), cancellationToken);

            if (product is null)
                return Result<GetBasketResponse>.Failure(ProductErrors.ProductNotFound);

            var basket = await basketRepository.GetOrCreateAsync(request.BuyerId, cancellationToken);

            var updateItemResult = basket.UpdateItemQuantity(product.Id, request.Quantity);

            if (updateItemResult.IsFailure)
                return Result<GetBasketResponse>.Failure(updateItemResult.Error!);

            await basketRepository.SaveAsync(basket, cancellationToken);
            return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
        }
    }
}