using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.AddBasketItem
{
    public sealed record AddBasketItemCommand(Guid BuyerId, int ProductId, int Quantity)
        : IRequest<Result<GetBasketResponse>>;
}