using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public sealed class MarkAllNotificationsAsReadCommandHandler
        : IRequestHandler<MarkAllNotificationsAsReadCommand, Result>
    {
        private readonly INotificationCommandService _service;

        public MarkAllNotificationsAsReadCommandHandler(
            INotificationCommandService service)
            => _service = service;

        public Task<Result> Handle(
            MarkAllNotificationsAsReadCommand request,
            CancellationToken cancellationToken)
            => _service.MarkAllReadAsync(
                request.UserId,
                cancellationToken);
    }
}
