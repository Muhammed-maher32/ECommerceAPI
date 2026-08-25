using ECommerce.Domain.Shared;
using ECommerce.UseCases.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.DeliveryMethods.Queries.GetAllDeliveryMethods;

public class GetAllDeliveryMethodsQueryHandler(
    IDeliveryMethodQueryService deliveryMethodQueryService)
    : IRequestHandler<GetAllDeliveryMethodsQuery,
        Result<IReadOnlyList<GetAllDeliveryMethodsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllDeliveryMethodsResponse>>> Handle(GetAllDeliveryMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var methods = await deliveryMethodQueryService.GetAllDeliveryMethodsAsync(cancellationToken);

        return Result<IReadOnlyList<GetAllDeliveryMethodsResponse>>.Success(methods);
    }
}
