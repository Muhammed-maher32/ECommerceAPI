using ECommerce.API.Models;
using ECommerce.UseCases.Brands.Dtos;
using ECommerce.UseCases.Brands.Queries.GetAllBrands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(IMediator mediator) : ApiControllerBase
{
    // GET: api/brands
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllBrandsResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllBrandsQuery(), ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<IReadOnlyList<GetAllBrandsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}