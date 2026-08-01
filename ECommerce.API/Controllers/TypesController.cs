using ECommerce.UseCases.Types.Dtos;
using ECommerce.UseCases.Types.Queries.GetAllTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(IMediator mediator) : ApiControllerBase
{
    // GET: api/types
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetAllTypesResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllTypesQuery(), ct);
        return Ok(result.Value);
    }
}