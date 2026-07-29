namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentaion(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
}
