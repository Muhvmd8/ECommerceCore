using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.Handlers
{
    public sealed class RemoveBasketItemCommandHandler(
        IBasketRepository basketRepository)
        : IRequestHandler<RemoveBasketItemCommand, Result<GetBasketResponse>>
    {
        public async Task<Result<GetBasketResponse>> Handle(
            RemoveBasketItemCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetOrCreateAsync(
                request.BuyerId,
                cancellationToken);

            var removeItemResult = basket.RemoveItem(request.ProductId);

            if (removeItemResult.IsFailure)
                return Result<GetBasketResponse>.Failure(
                    removeItemResult.Error!);

            await basketRepository.SaveAsync(
                basket,
                cancellationToken);

            return Result<GetBasketResponse>.Success(
                GetBasketResponse.From(basket));
        }
    }
}
