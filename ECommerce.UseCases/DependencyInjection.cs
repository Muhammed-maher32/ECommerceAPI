using ECommerce.UseCases.Products.Queries.Validators;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(typeof(GetPagedProductQueryValidtaor).Assembly);

        return services;
    }
}
