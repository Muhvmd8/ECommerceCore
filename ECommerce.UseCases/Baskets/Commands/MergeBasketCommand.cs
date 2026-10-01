using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands
{
    public sealed record MergeBasketCommand(Guid BuyerId, Guid AnonymousBuyerId)
        : IRequest<Result<GetBasketResponse>>;
}
