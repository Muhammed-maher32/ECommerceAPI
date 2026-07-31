using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;

namespace ECommerce.UseCases.Prdoucts.Queries;

public class GetByIdProductQuery(IProductQueryService productQueryService)
{
    public async Task<Result<GetByIdProductResponse>> ExecuteAsync(Guid id)
    {
        var product = await productQueryService.GetByIdProductAsync(id);

        if (product is null)
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);


        return Result<GetByIdProductResponse>.Success(product);
    }
}
