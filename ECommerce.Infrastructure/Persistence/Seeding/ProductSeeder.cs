using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public class ProductSeeder(StoreDbContext dbContext) : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync(CancellationToken ct = default)
    => await JsonSeeder.SeedIfEmpty<Product, ProductSeedModel>(
            dbContext.Products,
            "Products.json",
            p => Product.Create(
                p.Name,
                p.Description,
                p.PictureUrl,
                p.Price,
                p.ProductBrandId,
                p.ProductTypeId),
            ct);
}
