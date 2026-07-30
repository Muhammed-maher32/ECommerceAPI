namespace ECommerce.Infrastructure.Persistence.Seeding;

public interface IDataseeder
{
    int Order { get; }
    Task SeedAsync(CancellationToken ct = default);
}
