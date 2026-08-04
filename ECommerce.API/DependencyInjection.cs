using Asp.Versioning;
using ECommerce.API.Middlewares;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ECommerce.API.Filters.GlobalAuditLoggingFilter>();
        });

        services.AddProblemDetails();

        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0); // Default v1.0
            options.AssumeDefaultVersionWhenUnspecified = true; // Use v1 if client doesn't specify
            options.ReportApiVersions = true; // Returns api-supported-versions in HTTP headers
            options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Reads version from URL /v1/
        })
        .AddApiExplorer(options =>
        {
            // Formats version in Swagger/OpenAPI docs (e.g. 'v1')
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen(); //Generate OpenAPI file

        return services;
    }
}
