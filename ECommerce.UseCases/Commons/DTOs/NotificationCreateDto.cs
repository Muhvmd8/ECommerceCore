using ECommerce.Domain.Entities.NotificationModule;

namespace ECommerce.UseCases.Commons.DTOs
{
    public class NotificationCreateDto
    {
        public Guid UserId { get; set; } = default!;
        public NotificationType Type { get; set; }
        public string Message { get; set; } = default!;
        public object? Data { get; set; }
    }
}
