using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(int Id, Guid UserId)
        : IRequest<Result>;
}
