using ECommerce.Domain.Common;
using ECommerce.UseCases.Brands.Dtos;
using MediatR;

namespace ECommerce.UseCases.Brands.Queries.GetAllBrands;

public class GetAllBrandsQueryHandler(IBrandQueryService brandQueryService) :
    IRequestHandler<GetAllBrandsQuery, Result<IReadOnlyList<GetAllBrandsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> Handle(GetAllBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await brandQueryService.GetAllBrandsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllBrandsResponse>>.Success(brands);
    }
}
