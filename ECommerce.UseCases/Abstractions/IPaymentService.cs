using ECommerce.Domain.Shared;
using ECommerce.UseCases.Commons.DTOs;

namespace ECommerce.UseCases.Abstractions
{
    public interface IPaymentService
    {
        Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(
            long amountInSmallestUnit,
            string currency,
            Guid orderId,
            CancellationToken cancellationToken = default);

        Task<Result<PaymentIntentResult>> UpdatePaymentIntentAsync(
            string paymentIntentId,
            long amountInSmallestUnit,
            Guid orderId,
            CancellationToken cancellationToken = default);

        /// <summary>Verify Stripe-Signature and map to a typed webhook event.</summary>
        Result<StripeWebhookEvent> ParseWebhook(string jsonPayload, string stripeSignature);

        /// <summary>Called when payment_intent.succeeded.</summary>
        Task PaymentSucceeded(string paymentIntentId, CancellationToken cancellationToken = default);

        /// <summary>Called when payment_intent.payment_failed.</summary>
        Task PaymentFailed(string paymentIntentId, CancellationToken cancellationToken = default);
    }
}
