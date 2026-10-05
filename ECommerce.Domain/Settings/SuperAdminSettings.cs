namespace ECommerce.Domain.Settings
{
    public class SuperAdminSettings
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}