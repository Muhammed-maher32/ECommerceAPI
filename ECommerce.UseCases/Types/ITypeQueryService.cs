using ECommerce.UseCases.Types.Dtos;

namespace ECommerce.UseCases.Types;

public interface ITypeQueryService
{
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken cancellationToken = default);
}
