using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;

namespace ECommerce.UseCases.Prdoucts.Queries;

public sealed class GetByIdProductQuery(IProductQueryService productQueryservice)
{
    public async Task<Result<GetByIdProductResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productQueryservice.GetByIdProductAsync(id, cancellationToken);

        if (product is null)
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);

        return product;
    }
}
