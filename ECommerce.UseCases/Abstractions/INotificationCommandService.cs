using ECommerce.Domain.Shared;
using ECommerce.UseCases.Commons.DTOs;

namespace ECommerce.UseCases.Abstractions
{
    public interface INotificationCommandService
    {
        Task<Result<int>> SendAsync(
            NotificationCreateDto dto,
            CancellationToken cancellationToken = default);

        Task<Result> MarkAsReadAsync(
            int id,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Result> MarkAllReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(
            int id,
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
