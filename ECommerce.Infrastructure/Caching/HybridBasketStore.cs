using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;

namespace ECommerce.Infrastructure.Caching;

public class HybridBasketStore(ICachedAggregateStore cacheStore)
{
    private const string BasketTag = "baskets";

    private static string GetBasketKey(Guid buyerId) => $"basket:{buyerId}";

    public async Task<Basket?> GetBasketAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        return await cacheStore.GetAsync<Basket>(key, ct);
    }

    public async Task<Basket> UpdateBasketAsync(Basket basket, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        var key = GetBasketKey(basket.BuyerId);
        var expiration = ttl ?? TimeSpan.FromDays(30);
        await cacheStore.SetAsync(key, basket, expiration, [BasketTag], ct);
        return basket;
    }

    public async Task DeleteBasketAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        await cacheStore.RemoveAsync(key, ct);
    }

    public async Task ClearAllBasketsAsync(CancellationToken ct = default)
    {
        await cacheStore.RemoveByTagAsync(BasketTag, ct);
    }
}
