using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderResponse>>;
