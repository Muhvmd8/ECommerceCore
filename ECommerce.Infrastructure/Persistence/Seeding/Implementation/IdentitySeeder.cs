using ECommerce.Domain.Constants;
using ECommerce.Domain.Settings;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Persistence.Seeding.Implementation
{
    public sealed class IdentitySeeder(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IOptions<SuperAdminSettings> superAdminSettings)
        : IDataSeeder
    {
        private readonly SuperAdminSettings _superAdminSettings = superAdminSettings.Value;

        public int Order => 0;

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            await SeedRolesAsync(ct);
            await SeedSuperAdminAsync(ct);
        }

        private async Task SeedRolesAsync(CancellationToken ct = default)
        {
            if (await roleManager.Roles.AnyAsync(ct))
                return;

            foreach (var role in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role,
                        Description = $"{role} system role"
                    });
            }
        }
        private async Task SeedSuperAdminAsync(CancellationToken ct = default)
        {
            if (await userManager.Users.AnyAsync(ct))
                return;

            var superAdminUser = new ApplicationUser
            {
                Email = _superAdminSettings.Email,
                EmailConfirmed = true,
                DisplayName = _superAdminSettings.DisplayName,
                UserName = _superAdminSettings.UserName
            };
            var result = await userManager.CreateAsync(superAdminUser, _superAdminSettings.Password);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(superAdminUser, Roles.SuperAdmin);
            else
                throw new Exception($"Failed to create super admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}