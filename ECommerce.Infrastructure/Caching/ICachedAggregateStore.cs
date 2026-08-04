namespace ECommerce.Infrastructure.Caching;

public interface ICachedAggregateStore
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan? expiration = null,
        IReadOnlyCollection<string>? tags = null,
        CancellationToken ct = default);

    Task RemoveAsync(string key, CancellationToken ct = default);

    Task RemoveByTagAsync(string tag, CancellationToken ct = default);
}
