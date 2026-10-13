using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Commands.SendNotification
{
    public sealed class SendNotificationCommandHandler
        : IRequestHandler<SendNotificationCommand, Result<int>>
    {
        private readonly INotificationCommandService _service;

        public SendNotificationCommandHandler(
            INotificationCommandService service)
        {
            _service = service;
        }

        public Task<Result<int>> Handle(
            SendNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var dto = new NotificationCreateDto
            {
                UserId = request.UserId,
                Type = request.Type,
                Message = request.Message,
                Data = request.DataJson
            };
            return _service.SendAsync(dto, cancellationToken);
        }
    }
}
