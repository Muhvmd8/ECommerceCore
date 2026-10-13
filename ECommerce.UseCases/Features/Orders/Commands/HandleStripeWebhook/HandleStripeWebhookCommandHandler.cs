using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.HandleStripeWebhook;

public sealed class HandleStripeWebhookCommandHandler(
    IPaymentService paymentService,
    INotificationCommandService notificationService,
    IReadRepository<Order> orderRepository)
    : IRequestHandler<HandleStripeWebhookCommand, Result>
{
    private const string PaymentIntentSucceeded = "payment_intent.succeeded";
    private const string PaymentIntentPaymentFailed = "payment_intent.payment_failed";

    public async Task<Result> Handle(
        HandleStripeWebhookCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Signature))
            return Result.Failure(OrderErrors.InvalidWebhook);

        var parsed = paymentService.ParseWebhook(
            request.Payload,
            request.Signature);

        if (parsed.IsFailure)
            return Result.Failure(parsed.Error!);

        var stripeEvent = parsed.Value;

        switch (stripeEvent.EventType)
        {
            case PaymentIntentSucceeded:
                if (!string.IsNullOrWhiteSpace(stripeEvent.PaymentIntentId))
                    await paymentService.PaymentSucceeded(
                        stripeEvent.PaymentIntentId,
                        cancellationToken);
                break;

            case PaymentIntentPaymentFailed:
                if (!string.IsNullOrWhiteSpace(stripeEvent.PaymentIntentId))
                    await paymentService.PaymentFailed(
                        stripeEvent.PaymentIntentId,
                        cancellationToken);
                break;
        }

        if (!string.IsNullOrWhiteSpace(stripeEvent.PaymentIntentId))
        {
            var orderResult = await orderRepository
                .FirstOrDefaultAsync(
                    new OrderByPaymentIntentIdSpecification(stripeEvent.PaymentIntentId),
                    cancellationToken);

            if (orderResult is not null)
            {
                var order = orderResult;

                var notificationType = stripeEvent.EventType switch
                {
                    PaymentIntentSucceeded => NotificationType.OrderConfirmed,
                    PaymentIntentPaymentFailed => NotificationType.PaymentFailed,
                    _ => (NotificationType?)null
                };

                if (notificationType.HasValue)
                {
                    await notificationService.SendAsync(
                        new NotificationCreateDto
                        {
                            UserId = order.UserId,
                            Type = notificationType.Value,
                            Message = notificationType.Value == NotificationType.OrderConfirmed
                                ? $"Your order {order.Id} has been confirmed successfully."
                                : $"Payment for order {order.Id} failed.",
                            Data = new
                            {
                                OrderId = order.Id,
                                PaymentIntentId = stripeEvent.PaymentIntentId
                            }
                        },
                        cancellationToken);
                }
            }
        }

        return Result.Success();
    }
}