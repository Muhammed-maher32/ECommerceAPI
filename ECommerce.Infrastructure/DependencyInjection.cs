using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using ECommerce.UseCases.CloudinaryPictureService;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.ProductTypes;
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
        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        // Since IEnumerable<IDataSeeder> seeders is registered,
        // EF Core / DI resolves all seeders in order as an IEnumerable list
        services.AddScoped<DataBaseSeeder>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();
        services.Configure<CloudinarySettings>(options =>
            config.GetSection(CloudinarySettings.SectionName).Bind(options));
        services.AddScoped<IPhotoService, CloudinaryPhotoService>();

        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHybridCachingInfrastructure(config);

        return services;
    }
}
