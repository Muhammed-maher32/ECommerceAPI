namespace ECommerce.UseCases.Baskets.Dtos;

public record BasketItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
