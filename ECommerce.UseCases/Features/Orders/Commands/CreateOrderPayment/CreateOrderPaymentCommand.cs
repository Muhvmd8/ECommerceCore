using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrderPayment;

public sealed record CreateOrderPaymentCommand(Guid OrderId)
    : IRequest<Result<PaymentClientSecretResponse>>;
