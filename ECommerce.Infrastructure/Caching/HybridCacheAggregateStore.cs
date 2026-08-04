using ECommerce.Domain.Repositories;
using Microsoft.Extensions.Caching.Hybrid;

namespace ECommerce.Infrastructure.Caching;

public class HybridCacheAggregateStore(HybridCache hybridCache) : ICachedAggregateStore
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(key)) return default;

        var envelope = await hybridCache.GetOrCreateAsync<CacheEnvelope<T>?>(
            key,
            async token => await ValueTask.FromResult<CacheEnvelope<T>?>(null),
            cancellationToken: ct);

        return envelope != null ? envelope.Data : default;
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        IReadOnlyCollection<string>? tags = null,
        CancellationToken ct = default)
    {
        var policy = CacheEntryPolicy.Create(expiration ?? TimeSpan.FromMinutes(10), tags: tags?.ToArray() ?? Array.Empty<string>());
        CacheEntryPolicyValidator.Validate(key, policy);

        var envelope = CacheEnvelope<T>.Wrap(value);

        var options = new HybridCacheEntryOptions
        {
            Expiration = policy.Expiration,
            LocalCacheExpiration = policy.LocalCacheExpiration
        };

        await hybridCache.SetAsync(key, envelope, options, tags, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            await hybridCache.RemoveAsync(key, ct);
        }
    }

    public async Task RemoveByTagAsync(string tag, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(tag))
        {
            await hybridCache.RemoveByTagAsync(tag, ct);
        }
    }

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

        var envelope = await hybridCache.GetOrCreateAsync(
            key,
            async token =>
            {
                var data = await factory(token);
                return CacheEnvelope<T>.Wrap(data);
            },
            options,
            tags,
            ct);

        return envelope.Data;
    }
}
