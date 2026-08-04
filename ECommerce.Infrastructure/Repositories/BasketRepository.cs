using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;

namespace ECommerce.Infrastructure.Repositories;

public class BasketRepository(HybridBasketStore basketStore) : IBasketRepository
{
    public async Task<Basket?> GetBasketAsync(Guid buyerId, CancellationToken ct = default)
    {
        return await basketStore.GetBasketAsync(buyerId, ct);
    }

    public async Task<Basket> UpdateBasketAsync(Basket basket, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        return await basketStore.UpdateBasketAsync(basket, ttl, ct);
    }

    public async Task DeleteBasketAsync(Guid buyerId, CancellationToken ct = default)
    {
        await basketStore.DeleteBasketAsync(buyerId, ct);
    }
}
