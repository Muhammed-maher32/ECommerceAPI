using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Caching;

public sealed class CacheEntryPolicyValidator : IValidateOptions<CacheEntryPolicy>
{
    public ValidateOptionsResult Validate(string? name, CacheEntryPolicy options)
    {
        // Only named policies are bound from configuration; the unnamed default instance
        // is never configured, so validating it would fail on values nobody set.
        if (string.IsNullOrEmpty(name))
            return ValidateOptionsResult.Skip;

        var failures = new List<string>();

        if (options.AbsoluteExpirationDays <= 0)
            failures.Add($"{nameof(options.AbsoluteExpirationDays)} must be greater than zero.");

        if (options.SlidingExpirationDays <= 0)
            failures.Add($"{nameof(options.SlidingExpirationDays)} must be greater than zero.");

        if (options.LocalCacheExpirationMinutes <= 0)
            failures.Add($"{nameof(options.LocalCacheExpirationMinutes)} must be greater than zero.");

        if (options.SlidingRefreshThresholdMinutes < 0)
            failures.Add($"{nameof(options.SlidingRefreshThresholdMinutes)} cannot be negative.");

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
