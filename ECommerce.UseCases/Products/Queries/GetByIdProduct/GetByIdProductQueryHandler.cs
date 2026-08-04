using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.Handlers;

public class GetByIdProductQueryHandler(IProductQueryService productQueryService) :
    IRequestHandler<GetByIdProductQuery, Result<GetByIdProductResponse>>
{
    public async Task<Result<GetByIdProductResponse>> Handle(GetByIdProductQuery request
        , CancellationToken cancellationToken)
    {
        var product = await productQueryService.GetByIdProductAsync(request.id, cancellationToken);

        if (product is null)
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);

        return product;
    }
}
