using ECommerce.Infrastructure.Persistence.DbContexts;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public sealed class DataBaseSeeder(StoreDbContext dbContext,
    IEnumerable<IDataSeeder> seeders)
{
    public async Task SeedAll(CancellationToken ct = default)
    {
        foreach (var seeder in seeders.OrderBy(s => s.Order))
        {
            await seeder.SeedAsync(ct);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}

