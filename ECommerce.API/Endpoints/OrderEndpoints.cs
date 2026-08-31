using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.API.Models;
using ECommerce.UseCases.Orders.Commands.CreateOrder;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Orders.Queries.GetOrderById;
using ECommerce.UseCases.Orders.Queries.GetOrders;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/orders")
            .WithTags("Orders")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .AddEndpointFilter<AuditEndpointFilter>()
            // Applied to the group, not per endpoint: a new order route added later is
            // authenticated by default rather than by remembering to say so.
            .RequireAuthorization();

        group.MapPost("/", async (
            CreateOrderRequest request,
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.User.GetUserId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var buyerEmail = httpContext.User.GetEmail();

            if (buyerEmail.IsFailure)
                return buyerEmail.Problem(httpContext);

            var result = await sender.Send(
                new CreateOrderCommand(
                    buyerId.Value,
                    buyerEmail.Value,
                    request.ShipToAddress,
                    request.DeliveryMethodId),
                cancellationToken);

            return result.FromResult(httpContext, "Order created successfully");
        })
        .WithSummary("Places an order from the current basket")
        .WithDescription(
            "The body carries no prices and no line items. Lines come from the server-side " +
            "basket and every price is re-read from the catalogue at checkout.")
        .Produces<ApiResponse<OrderResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/", async (
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.User.GetUserId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new GetOrdersQuery(buyerId.Value), cancellationToken);

            return result.FromResult(httpContext, "Orders retrieved successfully");
        })
        .WithSummary("Lists the caller's orders")
        .WithDescription("Always filtered by the id in the token; there is no buyer parameter.")
        .Produces<ApiResponse<IReadOnlyList<OrderSummaryResponse>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.User.GetUserId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new GetOrderByIdQuery(id, buyerId.Value), cancellationToken);

            return result.FromResult(httpContext, "Order retrieved successfully");
        })
        .WithSummary("Gets one of the caller's orders")
        .WithDescription(
            "An order belonging to someone else returns 404, not 403: a 403 would confirm " +
            "the id exists.")
        .Produces<ApiResponse<OrderResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
