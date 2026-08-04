using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IBasketRepository
{
    Task<Basket?> GetBasketAsync(Guid buyerId, CancellationToken ct = default);
    Task<Basket> UpdateBasketAsync(Basket basket, TimeSpan? ttl = null, CancellationToken ct = default);
    Task DeleteBasketAsync(Guid buyerId, CancellationToken ct = default);
}
