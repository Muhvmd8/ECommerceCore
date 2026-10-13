using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Infrastructure.Notifications.Specifications;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Notifications
{
    public sealed class NotificationCommandService
        : INotificationCommandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadRepository<Notification> _readRepository;
        private readonly INotificationRealTimeSender _realTimeSender;
        private readonly ILogger<NotificationCommandService> _logger;

        public NotificationCommandService(
            IUnitOfWork unitOfWork,
            IReadRepository<Notification> readRepository,
            INotificationRealTimeSender realTimeSender,
            ILogger<NotificationCommandService> logger)
        {
            _unitOfWork = unitOfWork;
            _readRepository = readRepository;
            _realTimeSender = realTimeSender;
            _logger = logger;
        }

        public async Task<Result<int>> SendAsync(
            NotificationCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            var createResult = Notification.Create(
                dto.UserId,
                dto.Type,
                dto.Message,
                dto.Data?.ToString());

            if (createResult.IsFailure)
                return Result<int>.Failure(createResult.Error!);

            var notification = createResult.Value;

            var writeRepository = _unitOfWork.GetRepository<Notification>();
            await writeRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            try
            {
                await _realTimeSender.SendToUserAsync(
                    dto.UserId,
                    new NotificationDto
                    {
                        Id = notification.Id,
                        Type = notification.Type,
                        Message = notification.Message,
                        DataJson = notification.DataJson,
                        IsRead = notification.IsRead,
                        CreatedAt = notification.CreatedAt
                    },
                    cancellationToken);
            }
            catch (Exception ex)
            {
                // The notification is already saved.
                // A real-time delivery failure must not undo that success.
                _logger.LogWarning(
                    ex,
                    "Failed to send notification {NotificationId} to user {UserId} in real time.",
                    notification.Id,
                    dto.UserId);
            }

            return Result<int>.Success(notification.Id);
        }

        public async Task<Result> MarkAsReadAsync(
            int id,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var notification = await _readRepository.FirstOrDefaultAsync(new NotificationByIdSpecification(id), cancellationToken);

            if (notification is null ||
                notification.ApplicationUserId != userId)
            {
                return Result.Failure(
                    NotificationErrors.NotificationNotFound);
            }

            if (notification.IsRead)
            {
                return Result.Failure(
                    NotificationErrors.NotificationAlreadyRead);
            }

            notification.MarkAsRead();

            var writeRepository = _unitOfWork.GetRepository<Notification>();
            writeRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> MarkAllReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var unread = await _readRepository.ListAsync(
                new GetAllUserNotificationsSpecification(userId),
                cancellationToken);

            foreach (var notification in unread)
                notification.MarkAsRead();

            if (unread.Count == 0)
                return Result.Success();

            var writeRepository = _unitOfWork.GetRepository<Notification>();
            writeRepository.UpdateRange(unread);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(
            int id,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var notification = await _readRepository.FirstOrDefaultAsync(
                new NotificationByIdSpecification(id),
                cancellationToken);

            if (notification is null ||
                notification.ApplicationUserId != userId)
            {
                return Result.Failure(
                    NotificationErrors.NotificationNotFound);
            }

            var writeRepository = _unitOfWork.GetRepository<Notification>();
            writeRepository.Delete(notification);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
