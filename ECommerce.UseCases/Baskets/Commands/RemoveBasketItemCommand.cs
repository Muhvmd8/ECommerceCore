using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands
{
    public sealed record RemoveBasketItemCommand(Guid BuyerId, int ProductId)
        : IRequest<Result<GetBasketResponse>>;
}
