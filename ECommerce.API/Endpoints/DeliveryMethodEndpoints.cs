using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.API.Models;
using ECommerce.UseCases.DeliveryMethods.Dtos;
using ECommerce.UseCases.DeliveryMethods.Queries.GetAllDeliveryMethods;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class DeliveryMethodEndpoints
{
    public static IEndpointRouteBuilder MapDeliveryMethodEndPoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersion)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/deliverymethods")
            .WithTags("deliverymethods")
            .WithApiVersionSet(apiVersion)
            .HasApiVersion(new ApiVersion(1, 0))
            .AddEndpointFilter<AuditEndpointFilter>();

        group.MapGet("/", async (
            ISender sender,
            HttpContext httpContext,
            CancellationToken ct
            ) =>
        {
            var result = await sender.Send(new GetAllDeliveryMethodsQuery(), ct);
            return result.FromResult(httpContext, "Delivery methods retrieved successfully");
        })
        .WithSummary("Gets all delivery methods")
        .WithDescription("Returns the shipping options a shopper can pick at checkout, cheapest first.")
        .CacheOutput("DeliveryMethods")
        .Produces<ApiResponse<IReadOnlyList<GetAllDeliveryMethodsResponse>>>(StatusCodes.Status200OK);

        return endpoints;
    }
}
