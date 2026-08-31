namespace ECommerce.UseCases.Orders.Dtos;

public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
