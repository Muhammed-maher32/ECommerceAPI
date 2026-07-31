using ECommerce.Infrastructure.Interceptors;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases.Prdoucts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration config,
        IHostEnvironment environment)
    {
        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddScoped<IDataseeder, ProductBrandSeeder>();
        services.AddScoped<IDataseeder, ProductTypeSeeder>();
        //Since (IEnumerable<IDataseeder>seeders)
        //Its IEnumerable it will send em in order cuz its ienumerable not just a single object
        services.AddScoped<DataBaseSeeder>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        return services;
    }
}
