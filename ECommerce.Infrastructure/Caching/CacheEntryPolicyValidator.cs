namespace ECommerce.Infrastructure.Caching;

public static class CacheEntryPolicyValidator
{
    public static void Validate(string key, CacheEntryPolicy policy)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
        }

        if (policy is null)
        {
            throw new ArgumentNullException(nameof(policy), "CacheEntryPolicy cannot be null.");
        }

        if (policy.Expiration.HasValue && policy.Expiration.Value <= TimeSpan.Zero)
        {
            throw new ArgumentException("Expiration duration must be greater than zero.", nameof(policy));
        }

        if (policy.LocalCacheExpiration.HasValue && policy.LocalCacheExpiration.Value <= TimeSpan.Zero)
        {
            throw new ArgumentException("Local cache expiration duration must be greater than zero.", nameof(policy));
        }

        if (policy.Expiration.HasValue && policy.LocalCacheExpiration.HasValue && policy.LocalCacheExpiration.Value > policy.Expiration.Value)
        {
            throw new ArgumentException("Local cache expiration cannot exceed total expiration.", nameof(policy));
        }
    }
}
