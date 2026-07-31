using ECommerce.Domain.Common;
using ECommerce.UseCases.Brands.Dtos;

namespace ECommerce.UseCases.Brands.Queries;

public sealed class GetAllBrandsQuery(IBrandQueryService brandQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var brands = await brandQueryService.GetAllBrandsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllBrandsResponse>>.Success(brands);
    }
}
