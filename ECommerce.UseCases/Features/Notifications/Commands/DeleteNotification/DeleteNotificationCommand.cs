using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.DeleteNotification
{
    // Commands/DeleteNotification/DeleteNotificationCommand.cs
    public sealed record DeleteNotificationCommand(int Id, Guid UserId)
        : IRequest<Result>;
}
