using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Orders.Dtos;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(
    ICurrentUserService currentUser,
    IReadRepository<Order> orderRepository)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<OrderResponse>.Failure(OrderErrors.Unauthorized);

        var order = await orderRepository.FirstOrDefaultAsync(
            new OrderByIdForUserSpecification(request.OrderId, currentUser.UserId.Value),
            cancellationToken);

        if (order is null)
            return Result<OrderResponse>.Failure(OrderErrors.NotFound);

        return Result<OrderResponse>.Success(OrderMappings.ToResponse(order));
    }
}
