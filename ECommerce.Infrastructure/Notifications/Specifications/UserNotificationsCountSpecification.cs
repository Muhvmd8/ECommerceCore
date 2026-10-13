using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    public sealed class UserNotificationsCountSpecification
        : Specification<Notification>
    {
        public UserNotificationsCountSpecification(Guid userId)
        {
            Query.Where(n => n.ApplicationUserId == userId);
        }
    }
}