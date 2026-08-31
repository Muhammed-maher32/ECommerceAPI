using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.ReadService;

public class ProductQueryService(StoreDbContext dbContext) : IProductQueryService
{
    public async Task<GetByIdProductResponse?> GetByIdProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .ProjectToType<GetByIdProductResponse>()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<Guid, ProductPricingSnapshot>> GetPricingSnapshotsAsync(
        IReadOnlyCollection<Guid> productIds,
        CancellationToken cancellationToken = default)
    {
        if (productIds.Count == 0)
            return new Dictionary<Guid, ProductPricingSnapshot>();

        // The soft-delete query filter applies here, so a delisted product simply
        // does not come back and checkout fails loudly instead of pricing it.
        return await dbContext.Products
            .AsNoTracking()
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new ProductPricingSnapshot(p.Id, p.Name, p.PictureUrl, p.Price))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}
