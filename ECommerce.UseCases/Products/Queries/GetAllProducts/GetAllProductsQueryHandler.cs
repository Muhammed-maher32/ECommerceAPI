using ECommerce.Domain.Common;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IProductQueryService productQueryService) :
    IRequestHandler<GetAllProductsQuery, Result<IReadOnlyList<GetAllProductsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllProductsResponse>>.Success(products);
    }
}
