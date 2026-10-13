using ECommerce.UseCases.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Hubs.Notifications
{
    public sealed class NotificationRealTimeSender
        : INotificationRealTimeSender
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationRealTimeSender(
            IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public Task SendToUserAsync(
            Guid userId,
            object payload,
            CancellationToken cancellationToken = default)
        {
            return _hub.Clients.User(userId.ToString())
                .SendAsync(
                    "ReceiveNotification",
                    payload,
                    cancellationToken);
        }
    }
}