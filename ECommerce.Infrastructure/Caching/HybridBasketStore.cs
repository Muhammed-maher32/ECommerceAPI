using ECommerce.Domain.Entities;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;

namespace ECommerce.Infrastructure.Caching;

public sealed class HybridBasketStore(ICachedAggregateStore<Basket> store) : IBasketStore
{
    // Striped gates instead of one-per-buyer so the set stays bounded; a buyer always
    // maps to the same gate, and unrelated buyers only collide by hash.
    private const int LockStripeCount = 64;

    private static readonly SemaphoreSlim[] LockStripes =
        [.. Enumerable.Range(0, LockStripeCount).Select(_ => new SemaphoreSlim(1, 1))];

    public Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default) =>
        store.GetAsync(BuildCacheKey(buyerId), ct);

    public Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default) =>
        store.GetOrCreateAsync(
            BuildCacheKey(buyerId),
            _ =>
            {
                var createResult = Basket.CreateEmpty(buyerId);

                if (createResult.IsFailure)
                    throw new InvalidOperationException(createResult.Error!.Message);

                return Task.FromResult(createResult.Value);
            },
            ct);

    public async Task<Result<Basket>> MutateAsync(
        Guid buyerId,
        Func<Basket, Result> mutate,
        bool createIfMissing = false,
        CancellationToken ct = default)
    {
        if (buyerId == Guid.Empty)
            return Result<Basket>.Failure(BasketErrors.InvalidBuyerId);

        var gate = GateFor(buyerId);

        await gate.WaitAsync(ct);

        try
        {
            Basket? basket;

            if (createIfMissing)
            {
                basket = await GetOrCreateAsync(buyerId, ct);
            }
            else
            {
                basket = await GetAsync(buyerId, ct);

                if (basket is null)
                    return Result<Basket>.Failure(BasketErrors.BasketNotFound);
            }

            var mutateResult = mutate(basket);

            if (mutateResult.IsFailure)
                return Result<Basket>.Failure(mutateResult.Error!);

            await SaveAsync(basket, ct);

            return Result<Basket>.Success(basket);
        }
        finally
        {
            gate.Release();
        }
    }

    public Task SaveAsync(Basket basket, CancellationToken ct = default) =>
        store.SetAsync(BuildCacheKey(basket.BuyerId), basket, ct);

    public Task DeleteAsync(Guid buyerId, CancellationToken ct = default) =>
        store.RemoveAsync(BuildCacheKey(buyerId), ct);

    private static SemaphoreSlim GateFor(Guid buyerId) =>
        LockStripes[(uint)buyerId.GetHashCode() % LockStripeCount];

    private static string BuildCacheKey(Guid buyerId) => $"basket:{buyerId}";
}
