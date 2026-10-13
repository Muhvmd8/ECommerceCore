using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class NotificationErrors
    {
        public static readonly Error NotificationNotFound =
            Error.NotFound(
                "Notification.NotFound",
                "The notification was not found.");

        public static readonly Error NotificationsNotFound =
            Error.NotFound(
                "Notification.NotificationsNotFound",
                "No notifications were found for the specified user.");

        public static readonly Error NotificationAccessDenied =
            Error.Forbidden(
                "Notification.AccessDenied",
                "You are not allowed to access this notification.");

        public static readonly Error NotificationAlreadyRead =
            Error.Conflict(
                "Notification.AlreadyRead",
                "The notification has already been marked as read.");

        public static readonly Error InvalidNotificationType =
            Error.Validation(
                "Notification.InvalidType",
                "The specified notification type is invalid.");

        public static readonly Error NotificationMessageRequired =
            Error.Validation(
                "Notification.MessageRequired",
                "The notification message is required.");

        public static readonly Error ApplicationUserIdRequired =
            Error.Validation(
                "Notification.ApplicationUserIdRequired",
                "A valid application user id is required.");

        public static readonly Error NotificationCreationFailed =
            Error.Failure(
                "Notification.CreationFailed",
                "Failed to create the notification.");

        public static readonly Error NotificationMarkAsReadFailed =
            Error.Failure(
                "Notification.MarkAsReadFailed",
                "Failed to mark the notification as read.");

        public static readonly Error NotificationDeletionFailed =
            Error.Failure(
                "Notification.DeletionFailed",
                "Failed to delete the notification.");

        public static readonly Error InvalidPagination =
            Error.Validation(
                "Notification.InvalidPagination",
                "Page number must be greater than zero and page size must be between 1 and 100.");
    }
}