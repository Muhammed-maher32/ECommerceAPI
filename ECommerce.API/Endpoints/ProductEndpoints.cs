using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.API.Models;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet
        )
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/products")
            .WithTags("Products")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .AddEndpointFilter<AuditEndpointFilter>();

        //Paged
        //GET /api/v1/products/paged

        group.MapGet("/paged", async (
            [AsParameters] GetPagedProductQuery query,
            ISender sender,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var result = await sender.Send(query, ct);

            return result.FromPagedResult(httpContext,
                query.PageNumber,
                query.PageSize,
                "Products retrieved successfully");
        })
            .WithSummary("Paginated Results.")
            .WithDescription("Returns a paginated list of products with filtering and sorting options")
            .CacheOutput("Products")
            .Produces<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>(StatusCodes.Status200OK);

        //GetById
        group.MapGet("/{id:guid}", async (
                Guid id,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetByIdProductQuery(id), cancellationToken);

            return result.FromResult(httpContext, "Product id retrieved successfully");
        })
            .WithName("GetProductById")
            .WithSummary("Gets product by ID")
            .WithDescription("Returns product information")
            .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);



        return endpoints;
    }

}
