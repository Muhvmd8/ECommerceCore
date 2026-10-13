namespace ECommerce.UseCases.Commons.Options
{
    public sealed class OtpSettings
    {
        public const string SectionName = "OtpSettings";
        public int Length { get; set; } = 6;
        public int ExpirationMinutes { get; set; } = 10;
        public int LocalCacheExpirationMinutes { get; set; } = 1;
    }
}