namespace ECommerce.UseCases.Commons.DTOs
{
    public sealed record StripeWebhookEvent(
        string EventType,
        string PaymentIntentId);
}
