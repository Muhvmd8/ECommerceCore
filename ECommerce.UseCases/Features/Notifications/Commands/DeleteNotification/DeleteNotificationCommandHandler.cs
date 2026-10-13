using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.DeleteNotification
{
    public sealed class DeleteNotificationCommandHandler
        : IRequestHandler<DeleteNotificationCommand, Result>
    {
        private readonly INotificationCommandService _service;

        public DeleteNotificationCommandHandler(
            INotificationCommandService service)
            => _service = service;

        public Task<Result> Handle(
            DeleteNotificationCommand request,
            CancellationToken cancellationToken)
            => _service.DeleteAsync(
                request.Id,
                request.UserId,
                cancellationToken);
    }
}
