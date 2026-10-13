namespace ECommerce.UseCases.Commons.DTOs
{
    public sealed record PaymentIntentResult(
        string PaymentIntentId,
        string ClientSecret,
        string Status,
        string PublishableKey);
}
