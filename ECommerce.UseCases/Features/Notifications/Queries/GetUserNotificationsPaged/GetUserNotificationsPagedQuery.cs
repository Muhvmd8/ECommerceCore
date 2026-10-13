using ECommerce.Domain.Shared;
using ECommerce.UseCases.Commons.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Notifications.Queries.GetUserNotificationsPaged
{
    public sealed record GetUserNotificationsPagedQuery(
        string UserId,
        int PageNumber = 1,
        int PageSize = 10)
        : IRequest<Result<PagedResult<NotificationDto>>>;
}
