using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.ProductBrands.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.ReadService;

public sealed class BrandQueryService(StoreDbContext dbContext) : IBrandQueryService
{
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Brands
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToListAsync(cancellationToken);
    }
}
