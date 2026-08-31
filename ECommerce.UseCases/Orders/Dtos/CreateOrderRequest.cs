namespace ECommerce.UseCases.Orders.Dtos;

/// <summary>
/// Note what is absent: no prices, no line items, no buyer id. The lines come from
/// the server-side basket and every price is re-read from the database, so a caller
/// cannot dictate what the order costs.
/// </summary>
public record CreateOrderRequest(
    ShippingAddressRequest ShipToAddress,
    Guid DeliveryMethodId);
