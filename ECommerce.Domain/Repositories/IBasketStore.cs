using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IBasketStore
{
    Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default);
    //if exits => Done
    //Not Exits => Add to cach
    Task SaveAsync(Basket basket, CancellationToken ct = default);
    Task DeleteAsync(Guid buyerId, CancellationToken ct = default);
}
