using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;

namespace ECommerce.UseCases.Prdoucts.Queries;

public sealed class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllProductsResponse>>.Success(products);
    }
}
