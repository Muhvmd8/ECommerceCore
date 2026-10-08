using ECommerce.UseCases.Commons.Options;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Commons.Options.Validators
{
    public class JwtSettingsValidator : IValidateOptions<JwtSettings>
    {
        public ValidateOptionsResult Validate(
            string? name,
            JwtSettings options)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(options.SecretKey))
                errors.Add("JWT SecretKey is required.");

            if (options.SecretKey.Length < 32)
                errors.Add("JWT SecretKey must be at least 32 characters.");

            if (string.IsNullOrWhiteSpace(options.Issuer))
                errors.Add("JWT Issuer is required.");

            if (string.IsNullOrWhiteSpace(options.Audience))
                errors.Add("JWT Audience is required.");

            if (options.AccessTokenExpirationInMinutes <= 0)
                errors.Add("Access token expiration must be greater than zero.");

            if (options.RefreshTokenExpirationInDays <= 0)
                errors.Add("Refresh token expiration must be greater than zero.");

            return errors.Count > 0
                ? ValidateOptionsResult.Fail(errors)
                : ValidateOptionsResult.Success;
        }
    }
}
