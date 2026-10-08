using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.MergeBasket
{
    public sealed class MergeBasketCommandHandler(
        IBasketRepository basketRepository)
        : IRequestHandler<MergeBasketCommand, Result<GetBasketResponse>>
    {
        public async Task<Result<GetBasketResponse>> Handle(
            MergeBasketCommand request,
            CancellationToken cancellationToken)
        {
            var buyerBasket = await basketRepository.GetOrCreateAsync(request.BuyerId, cancellationToken);

            var anonymousBasket = await basketRepository.GetAsync(request.AnonymousBuyerId, cancellationToken);

            if (anonymousBasket is null || anonymousBasket.Items.Count == 0)
                return Result<GetBasketResponse>.Failure(BasketErrors.AnonymousBasketNotFound);

            var mergeResult = buyerBasket.MergeFrom(anonymousBasket);

            if (mergeResult.IsFailure)
                return Result<GetBasketResponse>.Failure(mergeResult.Error!);

            await basketRepository.SaveAsync(buyerBasket, cancellationToken);
            await basketRepository.DeleteAsync(request.AnonymousBuyerId, cancellationToken);

            return Result<GetBasketResponse>.Success(GetBasketResponse.From(buyerBasket));
        }
    }
}