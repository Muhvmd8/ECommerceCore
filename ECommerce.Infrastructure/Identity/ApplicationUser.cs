using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() => Id = Guid.NewGuid();

        #region Properties
        public string DisplayName { get; set; } = default!;
        #endregion
    }
}