using ECommerce.Domain.Common;
using ECommerce.UseCases.Brands;
using ECommerce.UseCases.Types.Dtos;
using MediatR;

namespace ECommerce.UseCases.Types.Queries.GetAllTypes;

public class GetAllTypesQueryHandler(ITypeQueryService typeQueryService) :
    IRequestHandler<GetAllTypesQuery, Result<IReadOnlyList<GetAllTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> Handle(GetAllTypesQuery request,
        CancellationToken cancellationToken)
    {
        var types = await typeQueryService.GetAllTypesAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}
