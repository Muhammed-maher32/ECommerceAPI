using ECommerce.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Caching;

public static class HybridCacheExtensions
{
    public static IServiceCollection AddHybridCachingInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
#pragma warning disable EXTEXP0018
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(15),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            };
        });
#pragma warning restore EXTEXP0018

        var redisConnectionString = config.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "ECommerce_";
            });
        }

        services.AddScoped<ICachedAggregateStore, HybridCacheAggregateStore>();
        services.AddScoped<HybridBasketStore>();

        return services;
    }
}
