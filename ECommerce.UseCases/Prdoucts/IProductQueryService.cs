using ECommerce.UseCases.Prdoucts.Dtos;

namespace ECommerce.UseCases.Prdoucts;

public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<GetByIdProductResponse?> GetByIdProductAsync(Guid id, CancellationToken cancellationToken = default);
}
