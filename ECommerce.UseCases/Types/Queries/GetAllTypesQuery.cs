using ECommerce.Domain.Common;
using ECommerce.UseCases.Types.Dtos;

namespace ECommerce.UseCases.Types.Queries;

public class GetAllTypesQuery(ITypeQueryService typeQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var types = await typeQueryService.GetAllTypesAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}
