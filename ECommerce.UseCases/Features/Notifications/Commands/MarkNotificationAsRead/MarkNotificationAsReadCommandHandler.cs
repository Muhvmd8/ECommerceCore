using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.MarkNotificationAsRead
{
    public sealed class MarkNotificationAsReadCommandHandler
        : IRequestHandler<MarkNotificationAsReadCommand, Result>
    {
        private readonly INotificationCommandService _service;

        public MarkNotificationAsReadCommandHandler(
            INotificationCommandService service)
            => _service = service;

        public Task<Result> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
            => _service.MarkAsReadAsync(
                request.Id,
                request.UserId,
                cancellationToken);
    }
}
