namespace ECommerce.Domain.Entities.NotificationModule
{
    public enum NotificationType
    {
        OrderPlaced = 1,
        OrderConfirmed = 2,
        OrderShipped = 3,
        OrderDelivered = 4,
        OrderCancelled = 5,

        PaymentIntentCreated = 6,
        PaymentSucceeded = 7,
        PaymentFailed = 8,
        RefundProcessed = 9,

        ProductPriceChanged = 10,
        ProductBackInStock = 11,

        ReviewReceived = 12,

        AccountSecurity = 13,
        General = 14
    }
}
