using Asp.Versioning;
using ECommerce.API.Middlewares;
using ECommerce.Infrastructure.Identity;
using ECommerce.UseCases.Shared.Settings;
using ECommerce.UseCases.Users.Commands.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services,
        IConfiguration config)
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

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the access token only -- Swagger adds the Bearer prefix."
            });

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        }); //Generate OpenAPI file

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Kept deliberately in sync with PasswordRules in ECommerce.UseCases. Spelled out
            // rather than left to the Identity defaults so the two policies can be compared
            // line for line.
            options.Password.RequiredLength = PasswordRules.MinLength;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<IdentityStoreDbContext>()
            .AddDefaultTokenProviders(); // For Reset Password.


        // Read eagerly: the validation parameters below are built once at registration
        // time, so a missing section has to fail here rather than on the first request.
        var jwtSettings = config.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                $"The '{JwtSettings.SectionName}' configuration section is missing.");

        // AddIdentity above sets the cookie schemes as the defaults; this call runs
        // after it on purpose so bearer tokens win. Do not reorder the two.
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                    ValidateLifetime = true,

                    // No grace period: expiry is exact, which means the issuing and
                    // validating clocks have to agree (they do while both are this host).
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        return services;
    }
}
