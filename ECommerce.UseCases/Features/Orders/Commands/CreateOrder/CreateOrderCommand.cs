using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid ShippingAddressId,
    Guid DeliveryMethodId) : IRequest<Result<OrderResponse>>;
