using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Queries
{
    public sealed record GetBasketQuery(Guid BuyerId)
        : IRequest<Result<GetBasketResponse>>;
}
