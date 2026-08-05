using ECommerce.Domain.Entities;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Repositories;

public interface IBasketStore
{
    Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default);

    Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default);

    /// <summary>
    /// Loads the basket, applies <paramref name="mutate"/> and saves it back as a single
    /// serialized unit, so concurrent writes to the same buyer cannot overwrite each other.
    /// </summary>
    Task<Result<Basket>> MutateAsync(
        Guid buyerId,
        Func<Basket, Result> mutate,
        bool createIfMissing = false,
        CancellationToken ct = default);

    Task SaveAsync(Basket basket, CancellationToken ct = default);

    Task DeleteAsync(Guid buyerId, CancellationToken ct = default);
}
