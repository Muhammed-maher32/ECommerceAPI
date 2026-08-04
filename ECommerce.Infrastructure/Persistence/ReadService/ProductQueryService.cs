using ECommerce.Domain.Entities;
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
}

