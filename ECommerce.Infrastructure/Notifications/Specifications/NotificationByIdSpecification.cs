using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    internal class NotificationByIdSpecification
        : Specification<Notification>
    {
        public NotificationByIdSpecification(int id)
        {
            Query.Where(n => n.Id == id);
        }
    }
}
