using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    public sealed class GetUnreadNotificationsCountSpecification
        : Specification<Notification>
    {
        public GetUnreadNotificationsCountSpecification(Guid userId)
        {
            Query.Where(n =>
                n.ApplicationUserId == userId &&
                !n.IsRead);
        }
    }
}
