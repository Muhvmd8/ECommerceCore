using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Infrastructure.Notifications.Specifications;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;

namespace ECommerce.Infrastructure.Notifications
{
    public sealed class NotificationQueryService : INotificationQueryService
    {
        private readonly IReadRepository<Notification> _readRepository;

        public NotificationQueryService(
            IReadRepository<Notification> readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Result<PagedResult<NotificationDto>>>
            GetUserNotificationsPagedAsync(
                Guid userId,
                int pageNumber,
                int pageSize,
                CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                return Result<PagedResult<NotificationDto>>.Failure(NotificationErrors.ApplicationUserIdRequired);

            if (pageNumber < 1 || pageSize < 1)
                return Result<PagedResult<NotificationDto>>.Failure(NotificationErrors.InvalidPagination);

            var totalCount = await _readRepository.CountAsync(
                new GetUserNotificationsCountSpecification(userId),
                cancellationToken);

            var notifications = await _readRepository.ListAsync(
                new GetUserNotificationsPagedSpecification(
                    userId,
                    pageNumber,
                    pageSize),
                cancellationToken);

            var paginatedResult = Result<PagedResult<NotificationDto>>.Success(
                new PagedResult<NotificationDto>(
                    notifications,
                    totalCount));

            return paginatedResult;
        }

        public async Task<Result<int>> GetUnreadCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                return Result<int>.Failure(NotificationErrors.ApplicationUserIdRequired);

            var result = await _readRepository.CountAsync(
                new GetUnreadNotificationsCountSpecification(userId),
                cancellationToken);

            return Result<int>.Success(result);
        }
    }
}