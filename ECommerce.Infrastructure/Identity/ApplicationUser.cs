using ECommerce.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() => Id = Guid.NewGuid();
        public string DisplayName { get; set; } = default!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}