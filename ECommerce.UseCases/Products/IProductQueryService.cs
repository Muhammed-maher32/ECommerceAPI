using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products;

public interface IProductQueryService
{
    Task<GetByIdProductResponse?> GetByIdProductAsync(Guid id, CancellationToken cancellationToken = default);
}

