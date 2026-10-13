using ECommerce.Domain.Entities.NotificationModule;

namespace ECommerce.UseCases.Commons.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = default!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public string? DataJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
