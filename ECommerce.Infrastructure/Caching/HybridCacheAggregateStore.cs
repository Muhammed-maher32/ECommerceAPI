using Microsoft.Extensions.Caching.Hybrid;

namespace ECommerce.Infrastructure.Caching;

public class HybridCacheAggregateStore(HybridCache hybridCache) : ICachedAggregateStore
{
    public async Task<T> GetOrCreateAsync<T>(
       string key,
       Func<CancellationToken, ValueTask<T>> factory,
       TimeSpan? expiration = null,
       IReadOnlyCollection<string>? tags = null,
       CancellationToken ct = default)
    {
        var policy = CacheEntryPolicy.Create(expiration ?? TimeSpan.FromMinutes(10), tags: tags?.ToArray() ?? Array.Empty<string>());
        CacheEntryPolicyValidator.Validate(key, policy);

        var options = new HybridCacheEntryOptions
        {
            Expiration = policy.Expiration,
            LocalCacheExpiration = policy.LocalCacheExpiration
        };

        return await hybridCache.GetOrCreateAsync(
            key,
            factory,
            options,
            tags,
            ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(key))
            await hybridCache.RemoveAsync(key, ct);
    }

    public async Task RemoveByTagAsync(string tag, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(tag))
            await hybridCache.RemoveByTagAsync(tag, ct);
    }
}
