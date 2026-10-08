using ECommerce.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.Auth
{
    public class RefreshToken : BaseEntity<int>
    {
        public Guid ApplicationUserId { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string JwtId { get; set; } = default!;
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
        [NotMapped]
        public bool IsActive => !IsUsed && !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }
}
