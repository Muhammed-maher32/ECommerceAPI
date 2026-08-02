using ECommerce.API.Models;
using ECommerce.UseCases.Types.Dtos;
using ECommerce.UseCases.Types.Queries.GetAllTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(IMediator mediator) : ApiControllerBase
{
    // GET: api/types
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllTypesResponse>>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllTypesResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllTypesQuery(), ct);
        return result.IsFailure
            ? Problem(result)
            : Ok(ApiResponse<IReadOnlyList<GetAllTypesResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}