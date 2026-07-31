using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;

namespace ECommerce.UseCases.Prdoucts.Queries;

public sealed class GetByIdProductQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> ExecuteAsync()
    {
        var products = await productQueryService.GetAllProductsAsync();
        return Result<IReadOnlyList<GetAllProductsResponse>>.Success(products);
    }
}
