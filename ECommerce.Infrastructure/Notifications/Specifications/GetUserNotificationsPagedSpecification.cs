using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.UseCases.Commons.DTOs;
using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Notifications.Specifications
{
    public sealed class GetUserNotificationsPagedSpecification
        : Specification<Notification, NotificationDto>
    {
        public GetUserNotificationsPagedSpecification(
            Guid userId,
            int pageNumber,
            int pageSize)
        {
            Query
                .Where(n => n.ApplicationUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            Query
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type,
                    Message = n.Message,
                    DataJson = n.DataJson,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                });
        }
    }
}
