using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Orders.Dtos;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler(
    ICurrentUserService currentUser,
    IRepository<Order> orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<OrderResponse>.Failure(OrderErrors.Unauthorized);

        var order = await orderRepository.FirstOrDefaultAsync(
            new OrderByIdForUserSpecification(request.OrderId, currentUser.UserId.Value, tracking: true),
            cancellationToken);

        if (order is null)
            return Result<OrderResponse>.Failure(OrderErrors.NotFound);

        var cancelResult = order.Cancel();
        if (cancelResult.IsFailure)
            return Result<OrderResponse>.Failure(cancelResult.Error!);

        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrderResponse>.Success(OrderMappings.ToResponse(order));
    }
}
