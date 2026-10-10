using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest<Result<OrderResponse>>;
