using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Queries.Handlers
{
    public sealed class GetBasketQueryHandler(IBasketRepository basketRepository)
        : IRequestHandler<GetBasketQuery, Result<GetBasketResponse>>
    {
        public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetOrCreateAsync(request.BuyerId, cancellationToken);
            return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
        }
    }
}
