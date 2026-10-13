using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Queries.GetUnreadCount
{
    public sealed class GetUnreadCountQueryHandler
        : IRequestHandler<GetUnreadCountQuery, Result<int>>
    {
        private readonly INotificationQueryService _service;

        public GetUnreadCountQueryHandler(
            INotificationQueryService service)
            => _service = service;

        public Task<Result<int>> Handle(
            GetUnreadCountQuery request,
            CancellationToken cancellationToken)
            => _service.GetUnreadCountAsync(
                request.UserId,
                cancellationToken);
    }
}
