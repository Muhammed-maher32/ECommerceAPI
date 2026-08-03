using ECommerce.UseCases.ProductBrands.Dtos;

namespace ECommerce.UseCases.ProductBrands;

public interface IBrandQueryService
{
    Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
}
