namespace ECommerce.UseCases.Abstractions
{
    public interface INotificationRealTimeSender
    {
        Task SendToUserAsync(Guid userId, object payload, CancellationToken cancellationToken = default);
    }
}
