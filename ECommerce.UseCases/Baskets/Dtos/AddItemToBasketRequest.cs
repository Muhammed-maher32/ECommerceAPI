namespace ECommerce.UseCases.Baskets.Dtos;

public record AddItemToBasketRequest(
    Guid ProductId,
    int Quantity);
