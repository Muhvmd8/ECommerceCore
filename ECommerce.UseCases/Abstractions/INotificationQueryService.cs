using ECommerce.Domain.Shared;
using ECommerce.UseCases.Commons.DTOs;

namespace ECommerce.UseCases.Abstractions
{
    public interface INotificationQueryService
    {
        Task<Result<PagedResult<NotificationDto>>> GetUserNotificationsPagedAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<Result<int>> GetUnreadCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}