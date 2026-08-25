using ECommerce.UseCases.DeliveryMethods.Dtos;

namespace ECommerce.UseCases.DeliveryMethods;

public interface IDeliveryMethodQueryService
{
    Task<IReadOnlyList<GetAllDeliveryMethodsResponse>> GetAllDeliveryMethodsAsync(
        CancellationToken ct = default);
}
