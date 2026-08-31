namespace ECommerce.UseCases.Orders.Dtos;

/// <summary>
/// The list projection. Deliberately without the lines: the history page shows totals,
/// and loading every item of every order to add them up is the expensive way to do it.
/// </summary>
public record OrderSummaryResponse(
    Guid Id,
    DateTimeOffset OrderDate,
    string DeliveryMethodName,
    int TotalItems,
    decimal Subtotal,
    decimal DeliveryPrice,
    decimal Total,
    string Status);
