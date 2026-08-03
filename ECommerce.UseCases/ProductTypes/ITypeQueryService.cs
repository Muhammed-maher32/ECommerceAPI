using ECommerce.UseCases.ProductTypes.Dtos;

namespace ECommerce.UseCases.ProductTypes;

public interface ITypeQueryService
{
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken cancellationToken = default);
}
