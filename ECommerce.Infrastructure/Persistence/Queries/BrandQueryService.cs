using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Brands;
using ECommerce.UseCases.Brands.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public sealed class BrandQueryService(StoreDbContext dbContext) : IBrandQueryService
{
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Brands
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToListAsync(cancellationToken);
    }
}
