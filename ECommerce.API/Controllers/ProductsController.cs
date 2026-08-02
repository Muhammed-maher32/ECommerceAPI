using ECommerce.API.Models;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries.GetAllProducts;
using ECommerce.UseCases.Products.Queries.GetByIdProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllProductsResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(), ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<IReadOnlyList<GetAllProductsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
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
