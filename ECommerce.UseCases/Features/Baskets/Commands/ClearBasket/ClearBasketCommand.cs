using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Baskets.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Baskets.Commands.ClearBasket;

public sealed record ClearBasketCommand(Guid BuyerId) : IRequest<Result<GetBasketResponse>>;