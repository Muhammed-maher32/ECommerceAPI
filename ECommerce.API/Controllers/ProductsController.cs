using ECommerce.API.Models;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("paged")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllProductsResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> Paged(
        [FromQuery] GetPagedProductQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);

        return result.IsFailure
            ? Problem(result)
            : FromPagedResult(result, query.PageNumber, query.PageSize, "Paged products retrieved successfully");
    }

    //Get api/products/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetByIdProductResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GetByIdProductResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetByIdProductQuery(id), ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<GetByIdProductResponse>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}
