using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging();
        });
        services.AddScoped<IDataseeder, ProductBrandSeeder>();
        services.AddScoped<IDataseeder, ProductTypeSeeder>();
        //Since (IEnumerable<IDataseeder>seeders)
        //Its IEnumerable it will send em in order cuz its ienumerable not just a single object
        services.AddScoped<DataBaseSeeder>();
        return services;
    }
}
