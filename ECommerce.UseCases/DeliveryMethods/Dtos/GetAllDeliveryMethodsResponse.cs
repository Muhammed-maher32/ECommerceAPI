namespace ECommerce.UseCases.DeliveryMethods.Dtos;

public record GetAllDeliveryMethodsResponse(string Name,
    string Description,
    string DeliveryTime,
    decimal Price);
