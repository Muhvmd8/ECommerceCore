using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.RemoveBasketItem
{
    public sealed record RemoveBasketItemCommand(Guid BuyerId, int ProductId)
        : IRequest<Result<GetBasketResponse>>;
}
