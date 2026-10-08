using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.MergeBasket
{
    public sealed record MergeBasketCommand(Guid BuyerId, Guid AnonymousBuyerId)
        : IRequest<Result<GetBasketResponse>>;
}
