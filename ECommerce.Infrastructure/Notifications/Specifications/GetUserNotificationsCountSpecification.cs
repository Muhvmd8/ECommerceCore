using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    public sealed class GetUserNotificationsCountSpecification
        : Specification<Notification>
    {
        public GetUserNotificationsCountSpecification(Guid userId)
        {
            Query.Where(n => n.ApplicationUserId == userId);
        }
    }
}
