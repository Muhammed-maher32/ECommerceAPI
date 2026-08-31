using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface IProductQueryService
{
    Task<GetByIdProductResponse?> GetByIdProductAsync(Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Batch price lookup for checkout. One round trip for the whole basket instead
    /// of one per line, and ids that no longer resolve are simply absent from the result.
    /// </summary>
    Task<IReadOnlyDictionary<Guid, ProductPricingSnapshot>> GetPricingSnapshotsAsync(
        IReadOnlyCollection<Guid> productIds,
        CancellationToken cancellationToken = default);
}
