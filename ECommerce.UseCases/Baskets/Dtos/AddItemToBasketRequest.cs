namespace ECommerce.UseCases.Baskets.Dtos;

public record AddItemToBasketRequest(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity);
