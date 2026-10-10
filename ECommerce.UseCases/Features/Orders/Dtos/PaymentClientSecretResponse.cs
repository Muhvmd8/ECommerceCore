namespace ECommerce.UseCases.Features.Orders.Dtos;

public sealed record PaymentClientSecretResponse(
    Guid OrderId,
    string PaymentIntentId,
    string ClientSecret,
    string Status,
    string PublishableKey);