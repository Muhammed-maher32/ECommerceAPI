using ECommerce.UseCases.Orders.Dtos;

namespace ECommerce.UseCases.Orders;

public interface IOrderQueryService
{
    Task<IReadOnlyList<OrderSummaryResponse>> GetOrdersForBuyerAsync(
        Guid buyerId,
        CancellationToken ct = default);

    /// <summary>
    /// Scoped by buyer on purpose: an order belonging to someone else must come back
    /// as null so the endpoint can answer 404 rather than confirming it exists.
    /// </summary>
    Task<OrderResponse?> GetOrderForBuyerAsync(
        Guid orderId,
        Guid buyerId,
        CancellationToken ct = default);
}
