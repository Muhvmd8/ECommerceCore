using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    internal class GetAllUserNotificationsSpecification
        : Specification<Notification>
    {
        public GetAllUserNotificationsSpecification(Guid userId)
        {
            Query.Where(n => n.ApplicationUserId == userId && !n.IsRead);
        }
    }
}
