using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Extensions.Caching.Hybrid;

namespace ECommerce.Infrastructure.Caching;

public class HybridBasketStore(HybridCache hybridCache) : IBasketStore
{
    private const string BasketTag = "baskets";

    private static string GetBasketKey(Guid buyerId) => $"basket:{buyerId}";

    public async Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        return await hybridCache.GetOrCreateAsync<Basket?>(
            key,
            _ => ValueTask.FromResult<Basket?>(null),
            cancellationToken: ct);
    }

    public async Task<Basket> SaveAsync(Basket basket, CancellationToken ct = default)
    {
        var key = GetBasketKey(basket.BuyerId);
        await hybridCache.SetAsync(key, basket,
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromDays(30) },
            [BasketTag], ct);
        return basket;
    }

    public async Task DeleteAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        await hybridCache.RemoveAsync(key, ct);
    }
}
