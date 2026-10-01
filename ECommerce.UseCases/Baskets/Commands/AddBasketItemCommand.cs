using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands
{
    public sealed record AddBasketItemCommand(Guid BuyerId, int ProductId, int Quantity)
        : IRequest<Result<GetBasketResponse>>;
}