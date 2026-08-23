using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Filters;

namespace ECommerce.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/users")
            .WithTags("Users")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .AddEndpointFilter<AuditEndpointFilter>();

        // TODO: register / login endpoints go here, returning AccessTokenResult
        // via IJwtTokenGenerator.

        return endpoints;
    }
}
