namespace ECommerce.UseCases.DeliveryMethods.Dtos;

public record GetAllDeliveryMethodsResponse(
    Guid Id,
    string Name,
    string Description,
    string DeliveryTime,
    decimal Price);
