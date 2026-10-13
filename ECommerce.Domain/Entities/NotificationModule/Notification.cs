using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities.NotificationModule
{
    public class Notification : BaseEntity<int>
    {
        public Guid ApplicationUserId { get; private set; } = default!;

        public NotificationType Type { get; private set; }

        public string Message { get; private set; } = default!;

        /// <summary>
        /// Optional JSON payload containing contextual data,
        /// such as related VideoId, PlaylistId, or FromUserId.
        /// </summary>
        public string? DataJson { get; private set; }
        public bool IsRead { get; private set; }

        private Notification()
        {
        }

        private Notification(
            Guid applicationUserId,
            NotificationType type,
            string message,
            string? dataJson)
        {
            ApplicationUserId = applicationUserId;
            Type = type;
            Message = message;
            DataJson = dataJson;
        }

        public static Result<Notification> Create(
            Guid applicationUserId,
            NotificationType type,
            string message,
            string? dataJson = null)
        {
            if (applicationUserId == Guid.Empty)
                return Result<Notification>.Failure(
                    NotificationErrors.ApplicationUserIdRequired);

            if (!Enum.IsDefined(type))
                return Result<Notification>.Failure(
                    NotificationErrors.InvalidNotificationType);

            if (string.IsNullOrWhiteSpace(message))
                return Result<Notification>.Failure(
                    NotificationErrors.NotificationMessageRequired);

            var notification = new Notification(
                applicationUserId,
                type,
                message,
                dataJson);

            return Result<Notification>.Success(notification);
        }

        public void MarkAsRead()
        {
            if (IsRead)
                return;

            IsRead = true;
        }
    }
}
