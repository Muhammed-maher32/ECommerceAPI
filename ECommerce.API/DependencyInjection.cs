using Asp.Versioning;
using ECommerce.API.Middlewares;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Minimal APIs only; Swashbuckle discovers them through the endpoint
        // API explorer rather than the MVC one.
        services.AddEndpointsApiExplorer();

        services.AddProblemDetails();

        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0); // Default v1.0
            // Required: [ApiVersion] on ApiControllerBase is not inherited by the derived
            // controllers, so they resolve as unversioned and only match via this default.
            options.AssumeDefaultVersionWhenUnspecified = true;
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

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 8;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<IdentityStoreDbContext>()
            .AddDefaultTokenProviders(); // For Reset Password.

        return services;
    }
}
