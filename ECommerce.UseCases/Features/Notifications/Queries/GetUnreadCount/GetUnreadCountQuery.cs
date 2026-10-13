using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Queries.GetUnreadCount
{
    public sealed record GetUnreadCountQuery(Guid UserId)
        : IRequest<Result<int>>;
}
