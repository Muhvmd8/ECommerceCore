using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.UserModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using ECommerce.UseCases.Features.Orders.Dtos;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    ICurrentUserService currentUser,
    IBasketRepository basketRepository,
    IReadRepository<UserAddress> addressRepository,
    IRepository<DeliveryMethod> deliveryMethodRepository,
    IRepository<Order> orderRepository,
    IUnitOfWork unitOfWork,
    INotificationCommandService notificationService)
    : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<OrderResponse>.Failure(OrderErrors.Unauthorized);

        var userId = currentUser.UserId.Value;

        var address = await addressRepository.FirstOrDefaultAsync(
            new UserAddressByIdForUserSpecification(request.ShippingAddressId, userId),
            cancellationToken);

        if (address is null)
            return Result<OrderResponse>.Failure(OrderErrors.ShippingAddressNotFound);

        var deliveryMethod = await deliveryMethodRepository.FirstOrDefaultAsync(
            new DeliveryMethodEntityByIdSpecification(request.DeliveryMethodId),
            cancellationToken);

        if (deliveryMethod is null)
            return Result<OrderResponse>.Failure(DeliveryMethodErrors.NotFound);

        var basket = await basketRepository.GetAsync(userId, cancellationToken);
        if (basket is null || basket.Items.Count == 0)
            return Result<OrderResponse>.Failure(OrderErrors.EmptyBasket);

        var lines = basket.Items
            .Select(i => (
                i.ProductId,
                i.ProductName,
                i.PictureUrl,
                i.UnitPrice,
                i.Quantity
            )).ToList();

        var createResult = Order.Create(
            Guid.NewGuid(),
            userId,
            deliveryMethod,
            address,
            lines);

        if (createResult.IsFailure)
            return Result<OrderResponse>.Failure(createResult.Error!);

        var order = createResult.Value;
        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        basket.Clear();
        await basketRepository.SaveAsync(basket, cancellationToken);

        await notificationService.SendAsync(new NotificationCreateDto
        {
            UserId = userId,
            Type = NotificationType.OrderPlaced,
            Message = $"Your order {order.Id} has been created successfully.",
            Data = new { OrderId = order.Id }
        }, cancellationToken);

        return Result<OrderResponse>.Success(OrderMappings.ToResponse(order));
    }
}
