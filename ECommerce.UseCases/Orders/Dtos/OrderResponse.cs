namespace ECommerce.UseCases.Orders.Dtos;

public record OrderResponse(
    Guid Id,
    string BuyerEmail,
    DateTimeOffset OrderDate,
    ShippingAddressResponse ShipToAddress,
    string DeliveryMethodName,
    decimal DeliveryPrice,
    IReadOnlyList<OrderItemResponse> Items,
    decimal Subtotal,
    decimal Total,
    string Status,
    string? PaymentIntentId);
