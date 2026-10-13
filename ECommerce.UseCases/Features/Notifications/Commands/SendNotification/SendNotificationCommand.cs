using ECommerce.Domain.Entities.NotificationModule;
using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.SendNotification
{
    public sealed record SendNotificationCommand(
        Guid UserId,
        NotificationType Type,
        string Message,
        string? DataJson = null) : IRequest<Result<int>>;
}