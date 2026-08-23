using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.ReadService;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Settings;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.ProductTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration config,
        IHostEnvironment environment)
    {
        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__ApplicationMigrationsHistory"))
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddDbContext<IdentityStoreDbContext>((sp, options) =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__IdentityMigrationsHistory"))
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>());

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IDataSeeder, IdentitySeeder>();

        // Since IEnumerable<IDataSeeder> seeders is registered,
        // EF Core / DI resolves all seeders in order as an IEnumerable list
        services.AddScoped<DataBaseSeeder>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();


        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        AddJwt(services, config);

        AddBasketCaching(services, config);

        return services;
    }

    private static void AddJwt(IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<JwtSettings>()
            .Bind(config.GetSection(JwtSettings.SectionName))
            // HMAC-SHA256 rejects keys shorter than its 256-bit output, and it does so
            // at the first signing attempt. Fail at startup instead.
            .Validate(
                settings => Encoding.UTF8.GetByteCount(settings.Secret ?? string.Empty) >= 32,
                "Jwt:Secret must be at least 32 bytes long.")
            .Validate(
                settings => !string.IsNullOrWhiteSpace(settings.Issuer),
                "Jwt:Issuer is required.")
            .Validate(
                settings => !string.IsNullOrWhiteSpace(settings.Audience),
                "Jwt:Audience is required.")
            .Validate(
                settings => settings.AccessTokenExpirationMinutes > 0,
                "Jwt:AccessTokenExpirationMinutes must be greater than zero.")
            .ValidateOnStart();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    private static void AddBasketCaching(IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<CacheEntryPolicy>("Basket")
            .Bind(config.GetSection("CachedAggregates:Basket"))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CacheEntryPolicy>, CacheEntryPolicyValidator>();

        var redisConnection = config.GetConnectionString("Redis")
            ?? config.GetConnectionString("redis");

        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = redisConnection);
        }

        services.AddHybridCache();

        services.AddScoped(typeof(ICachedAggregateStore<>), typeof(HybridCacheAggregateStore<>));
        services.AddScoped<IBasketStore, HybridBasketStore>();
    }
}
