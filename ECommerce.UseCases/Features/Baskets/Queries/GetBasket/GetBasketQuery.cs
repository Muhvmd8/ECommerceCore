using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Queries.GetBasket
{
    public sealed record GetBasketQuery(Guid BuyerId)
        : IRequest<Result<GetBasketResponse>>;
}
