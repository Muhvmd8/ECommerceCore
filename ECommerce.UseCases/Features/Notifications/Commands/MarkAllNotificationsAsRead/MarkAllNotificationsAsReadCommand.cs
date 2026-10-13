using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public sealed record MarkAllNotificationsAsReadCommand(
        Guid UserId) : IRequest<Result>;
}
