using ECommerce.Domain.Entities.Common;
using ECommerce.Infrastructure.Identity;

namespace ECommerce.Infrastructure.Email
{
    public class EmailVerificationCode : BaseEntity<int>
    {
        public string Code { get; set; } = default!;
        public DateTime ExpirationTime { get; set; }
        public Guid ApplicationUserId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
