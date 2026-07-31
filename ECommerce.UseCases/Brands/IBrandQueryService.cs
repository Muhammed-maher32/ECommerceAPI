using ECommerce.UseCases.Brands.Dtos;

namespace ECommerce.UseCases.Brands;

public interface IBrandQueryService
{
    Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
}
