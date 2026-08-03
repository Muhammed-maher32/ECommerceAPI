using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.ProductTypes;
using ECommerce.UseCases.ProductTypes.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Queries;

public sealed class TypeQueryService(StoreDbContext dbContext) : ITypeQueryService
{
    public async Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Types
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToListAsync(cancellationToken);
    }
}
