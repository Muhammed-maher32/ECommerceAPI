using ECommerce.UseCases.Brands.Queries;
using ECommerce.UseCases.Prdoucts.Queries;
using ECommerce.UseCases.Types.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetByIdProductQuery>();
        services.AddScoped<GetAllProductsQuery>();
        services.AddScoped<GetAllBrandsQuery>();
        services.AddScoped<GetAllTypesQuery>();
        return services;
    }
}
