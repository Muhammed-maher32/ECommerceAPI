namespace ECommerce.UseCases.Baskets.Dtos;

public record CustomerBasketResponse(
    Guid BuyerId,
    IReadOnlyList<BasketItemResponse> Items,
    int TotalItems,
    decimal SubTotal);
