using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.UpdateBasketItemQuantity
{
    public sealed record UpdateBasketItemQuantityCommand(Guid BuyerId, int ProductId, int Quantity)
        : IRequest<Result<GetBasketResponse>>;
}
