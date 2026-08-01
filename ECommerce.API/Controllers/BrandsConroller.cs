using ECommerce.UseCases.Brands.Dtos;
using ECommerce.UseCases.Brands.Queries.GetAllBrands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(IMediator mediator) : ApiControllerBase
{
    // GET: api/brands
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetAllBrandsResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllBrandsQuery(), ct);
        return Ok(result.Value);
    }
}