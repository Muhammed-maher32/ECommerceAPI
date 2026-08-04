using ECommerce.API.Models;
using ECommerce.UseCases.Baskets.Commands;
using ECommerce.UseCases.Baskets.Dtos;
using ECommerce.UseCases.Baskets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BasketsController(IMediator mediator) : ApiControllerBase
{
    // GET: api/v1/baskets/{buyerId}
    [HttpGet("{buyerId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerBasketResponse>>> GetBasket(Guid buyerId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBasketQuery(buyerId), ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<CustomerBasketResponse>.Ok(result.Value, HttpContext.TraceIdentifier));
    }

    // POST: api/v1/baskets/{buyerId}/items
    [HttpPost("{buyerId:guid}/items")]
    [ProducesResponseType(typeof(ApiResponse<CustomerBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerBasketResponse>>> AddItem(
        Guid buyerId,
        [FromBody] AddItemToBasketRequest request,
        CancellationToken ct = default)
    {
        var command = new AddItemToBasketCommand(
            buyerId,
            request.ProductId,
            request.ProductName,
            request.PictureUrl,
            request.UnitPrice,
            request.Quantity);

        var result = await mediator.Send(command, ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<CustomerBasketResponse>.Ok(result.Value, HttpContext.TraceIdentifier, "Item added to basket successfully"));
    }

    // PUT: api/v1/baskets/{buyerId}/items/{productId}
    [HttpPut("{buyerId:guid}/items/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerBasketResponse>>> UpdateItemQuantity(
        Guid buyerId,
        Guid productId,
        [FromBody] UpdateBasketItemQuantityRequest request,
        CancellationToken ct = default)
    {
        var command = new UpdateBasketItemQuantityCommand(buyerId, productId, request.Quantity);
        var result = await mediator.Send(command, ct);

        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<CustomerBasketResponse>.Ok(result.Value, HttpContext.TraceIdentifier, "Basket item quantity updated successfully"));
    }

    // DELETE: api/v1/baskets/{buyerId}/items/{productId}
    [HttpDelete("{buyerId:guid}/items/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CustomerBasketResponse>>> RemoveItem(
        Guid buyerId,
        Guid productId,
        CancellationToken ct = default)
    {
        var command = new RemoveItemFromBasketCommand(buyerId, productId);
        var result = await mediator.Send(command, ct);

        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<CustomerBasketResponse>.Ok(result.Value, HttpContext.TraceIdentifier, "Item removed from basket successfully"));
    }

    // DELETE: api/v1/baskets/{buyerId}
    [HttpDelete("{buyerId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBasket(Guid buyerId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new DeleteBasketCommand(buyerId), ct);
        return result.IsFailure ? Problem(result) : NoContent();
    }
}
