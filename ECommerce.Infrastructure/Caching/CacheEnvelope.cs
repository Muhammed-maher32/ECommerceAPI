namespace ECommerce.Infrastructure.Caching;

public sealed record CacheEnvelope<T>
{
    public T Data { get; init; } = default!;
    public DateTimeOffset CachedAt { get; init; } = DateTimeOffset.UtcNow;
    public string Version { get; init; } = "v1";

    public CacheEnvelope() { }

    public CacheEnvelope(T data, string version = "v1")
    {
        Data = data;
        CachedAt = DateTimeOffset.UtcNow;
        Version = version;
    }

    public static CacheEnvelope<T> Wrap(T data, string version = "v1") => new(data, version);
}
