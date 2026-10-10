using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedResult<OrderResponse>>>;
