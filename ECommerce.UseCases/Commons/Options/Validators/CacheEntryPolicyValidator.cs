using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Commons.Options.Validators
{
    public sealed class CacheEntryPolicyValidator
        : IValidateOptions<CacheEntryPolicy>
    {
        public ValidateOptionsResult Validate(string? name, CacheEntryPolicy options)
        {
            var failures = new List<string>();

            if (options.AbsoluteExpirationDays <= 0)
                failures.Add($"{nameof(options.AbsoluteExpirationDays)} must be greater than 0.");

            if (options.SlidingExpirationDays <= 0)
                failures.Add($"{nameof(options.SlidingExpirationDays)} must be greater than 0.");

            if (options.LocalCacheExpirationMinutes <= 0)
                failures.Add($"{nameof(options.LocalCacheExpirationMinutes)} must be greater than 0.");

            if (options.SlidingRefreshThresholdMinutes <= 0)
                failures.Add($"{nameof(options.SlidingRefreshThresholdMinutes)} must be greater than 0.");

            return failures.Count == 0
                ? ValidateOptionsResult.Success
                : ValidateOptionsResult.Fail(failures);
        }
    }
}