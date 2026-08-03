using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductTypes.Dtos;
using MediatR;

namespace ECommerce.UseCases.ProductTypes.Queries.Handlers;

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
