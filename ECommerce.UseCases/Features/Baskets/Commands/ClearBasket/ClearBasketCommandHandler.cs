using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.ClearBasket;

public sealed class ClearBasketCommandHandler(IBasketRepository basketRepository)
    : IRequestHandler<ClearBasketCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(
        ClearBasketCommand request,
        CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetOrCreateAsync(request.BuyerId, cancellationToken);
        basket.Clear();
        await basketRepository.SaveAsync(basket, cancellationToken);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
    }
}