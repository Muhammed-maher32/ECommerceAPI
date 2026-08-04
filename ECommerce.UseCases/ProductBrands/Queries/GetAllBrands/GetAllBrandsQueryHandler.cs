using ECommerce.Domain.Shared;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.ProductBrands.Dtos;
using ECommerce.UseCases.ProductBrands.Queries;
using MediatR;

namespace ECommerce.UseCases.ProductBrands.Queries.Handlers;

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
