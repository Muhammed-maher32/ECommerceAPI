namespace ECommerce.Infrastructure.Caching;

public sealed record CacheEntryPolicy
{
    public TimeSpan? Expiration { get; init; }
    public TimeSpan? LocalCacheExpiration { get; init; }
    public IReadOnlyCollection<string> Tags { get; init; } = Array.Empty<string>();

    public static CacheEntryPolicy Default => new CacheEntryPolicy
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(2)
    };

    public static CacheEntryPolicy ShortLived => new CacheEntryPolicy
    {
        Expiration = TimeSpan.FromMinutes(2),
        LocalCacheExpiration = TimeSpan.FromSeconds(30)
    };

    public static CacheEntryPolicy LongLived => new CacheEntryPolicy
    {
        Expiration = TimeSpan.FromHours(1),
        LocalCacheExpiration = TimeSpan.FromMinutes(10)
    };

    public static CacheEntryPolicy Create(TimeSpan expiration, TimeSpan? localCacheExpiration = null, params string[] tags)
    {
        return new CacheEntryPolicy
        {
            Expiration = expiration,
            LocalCacheExpiration = localCacheExpiration,
            Tags = tags
        };
    }
}
