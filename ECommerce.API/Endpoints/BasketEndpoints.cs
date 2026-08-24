using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.API.Models;
using ECommerce.UseCases.Baskets.Commands.AddBasketItem;
using ECommerce.UseCases.Baskets.Commands.ClearBasket;
using ECommerce.UseCases.Baskets.Commands.RemoveBasketItem;
using ECommerce.UseCases.Baskets.Commands.UpdateBasketItemQuantity;
using ECommerce.UseCases.Baskets.Dtos;
using ECommerce.UseCases.Baskets.Queries.GetBasket;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class BasketEndpoints
{
    public static IEndpointRouteBuilder MapBasketEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/baskets")
            .WithTags("Baskets")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .AddEndpointFilter<AuditEndpointFilter>();

        group.MapGet("/", async (
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.GetBuyerId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new GetBasketQuery(buyerId.Value), cancellationToken);

            return result.FromResult(httpContext, "Basket retrieved successfully");
        })
        .WithSummary("Gets the current basket")
        .WithDescription(
            "Signed-in shoppers are identified by their token; guests must send X-Buyer-Id.")
        .Produces<ApiResponse<CustomerBasketResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/items", async (
            AddItemToBasketRequest request,
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.GetBuyerId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new AddItemToBasketCommand(
                    buyerId.Value, request.ProductId, request.Quantity),
                cancellationToken);

            return result.FromResult(httpContext, "Item added to basket successfully");
        })
        .WithSummary("Adds an item to the basket")
        .WithDescription("Quantity is an increment. Prices are read from the database, never from the request.")
        .Produces<ApiResponse<CustomerBasketResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/items/{productId:guid}", async (
            Guid productId,
            UpdateBasketItemQuantityRequest request,
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.GetBuyerId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new UpdateBasketItemQuantityCommand(
                    buyerId.Value, productId, request.Quantity),
                cancellationToken);

            return result.FromResult(httpContext, "Basket item updated successfully");
        })
        .WithSummary("Sets the quantity of a basket item")
        .Produces<ApiResponse<CustomerBasketResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/items/{productId:guid}", async (
            Guid productId,
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.GetBuyerId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new RemoveItemFromBasketCommand(buyerId.Value, productId),
                cancellationToken);

            return result.FromResult(httpContext, "Item removed from basket successfully");
        })
        .WithSummary("Removes a single item from the basket")
        .Produces<ApiResponse<CustomerBasketResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/", async (
            ISender sender,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var buyerId = httpContext.GetBuyerId();

            if (buyerId.IsFailure)
                return buyerId.Problem(httpContext);

            var result = await sender.Send(
                new DeleteBasketCommand(buyerId.Value), cancellationToken);

            return result.FromResult(httpContext, "Basket cleared successfully");
        })
        .WithSummary("Clears the whole basket")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
