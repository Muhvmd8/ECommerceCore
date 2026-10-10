namespace ECommerce.UseCases.Commons.Options
{
    public class CloudinarySettings
    {
        public const string SectionName = "CloudinarySettings";
        public string CloudName { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
        public string ApiSecret { get; set; } = default!;
    }
}