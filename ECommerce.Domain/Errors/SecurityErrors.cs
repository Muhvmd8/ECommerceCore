using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class SecurityErrors
    {
        public static readonly Error IpBlocked =
            new("Security.IpBlocked",
                "Your IP is banned.",
                ErrorType.Forbidden);

        public static readonly Error MaliciousPayloadDetected =
            new(
                "Security.MaliciousPayloadDetected",
                "Your IP was banned due to malicious activity.",
                ErrorType.Forbidden);
    }
}
