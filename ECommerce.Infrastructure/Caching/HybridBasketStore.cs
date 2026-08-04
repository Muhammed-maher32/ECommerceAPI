using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;

namespace ECommerce.Infrastructure.Caching;

public class HybridBasketStore(ICachedAggregateStore cacheStore) : IBasketStore
{
    private const string BasketTag = "baskets";

    private static string GetBasketKey(Guid buyerId) => $"basket:{buyerId}";

    public async Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        return await cacheStore.GetAsync<Basket>(key, ct);
    }

    public async Task<Basket> SaveAsync(Basket basket, CancellationToken ct = default)
    {
        var key = GetBasketKey(basket.BuyerId);
        await cacheStore.SetAsync(key, basket, TimeSpan.FromDays(30), [BasketTag], ct);
        return basket;
    }

    public async Task DeleteAsync(Guid buyerId, CancellationToken ct = default)
    {
        var key = GetBasketKey(buyerId);
        await cacheStore.RemoveAsync(key, ct);
    }

    public async Task ClearAllBasketsAsync(CancellationToken ct = default)
    {
        await cacheStore.RemoveByTagAsync(BasketTag, ct);
    }
}
