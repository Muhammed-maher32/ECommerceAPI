using ECommerce.UseCases.Prdoucts.Dtos;
using ECommerce.UseCases.Prdoucts.Queries.GetAllProducts;
using ECommerce.UseCases.Prdoucts.Queries.GetByIdProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


public class ProductsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetAllProductsResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(), ct);
        return Ok(result.Value);
    }

    //Get api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetByIdProductResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetByIdProductQuery(id), ct);

        return result.Match<ActionResult<GetByIdProductResponse>>(
                product => Ok(product),
                error => NotFound(error.Message)
            );
    }
}
