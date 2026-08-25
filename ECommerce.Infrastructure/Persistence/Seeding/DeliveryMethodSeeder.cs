using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

namespace ECommerce.Infrastructure.Persistence.Seeding;

internal class DeliveryMethodSeeder(StoreDbContext storeDbContext) : IDataSeeder
{
    public int Order => 4;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<DeliveryMethod, DeliveryMethodSeedModel>
        (storeDbContext.DeliveryMethods, "DeliveryMethods.json",
            d => DeliveryMethod.Create(d.Id, d.Name, d.Description, d.DeliveryTime, d.Price), ct);
    }
}