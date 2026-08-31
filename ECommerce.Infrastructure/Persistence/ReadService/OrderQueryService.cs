using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Orders;
using ECommerce.UseCases.Orders.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.ReadService;

public sealed class OrderQueryService(StoreDbContext dbContext) : IOrderQueryService
{
    public async Task<IReadOnlyList<OrderSummaryResponse>> GetOrdersForBuyerAsync(
        Guid buyerId,
        CancellationToken ct = default)
    {
        // Totals are computed in SQL from the lines; the entity's Subtotal/Total are
        // not mapped columns, so projecting them here is what keeps this one query.
        // SUM over numeric(18,2) widens the scale in Postgres, which would serialize
        // as 49.9800 here and 49.98 from the aggregate. Round so both agree.
        return await dbContext.Orders
            .AsNoTracking()
            .Where(order => order.BuyerId == buyerId)
            .OrderByDescending(order => order.OrderDate)
            .Select(order => new OrderSummaryResponse(
                order.Id,
                order.OrderDate,
                order.DeliveryMethodName,
                order.Items.Sum(item => item.Quantity),
                Math.Round(order.Items.Sum(item => item.UnitPrice * item.Quantity), 2),
                order.DeliveryPrice,
                Math.Round(order.Items.Sum(item => item.UnitPrice * item.Quantity) + order.DeliveryPrice, 2),
                order.Status.ToString()))
            .ToListAsync(ct);
    }

    public async Task<OrderResponse?> GetOrderForBuyerAsync(
        Guid orderId,
        Guid buyerId,
        CancellationToken ct = default)
    {
        // The buyer predicate is part of the lookup, not a check afterwards: there is
        // no code path here that can return another shopper's order.
        return await dbContext.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId && order.BuyerId == buyerId)
            .Select(order => new OrderResponse(
                order.Id,
                order.BuyerEmail,
                order.OrderDate,
                new ShippingAddressResponse(
                    order.ShipToAddress.RecipientFirstName,
                    order.ShipToAddress.RecipientLastName,
                    order.ShipToAddress.PhoneNumber,
                    order.ShipToAddress.Country,
                    order.ShipToAddress.City,
                    order.ShipToAddress.Street,
                    order.ShipToAddress.PostalCode),
                order.DeliveryMethodName,
                order.DeliveryPrice,
                order.Items
                    .Select(item => new OrderItemResponse(
                        item.ProductId,
                        item.ProductName,
                        item.PictureUrl,
                        item.UnitPrice,
                        item.Quantity,
                        item.UnitPrice * item.Quantity))
                    .ToList(),
                Math.Round(order.Items.Sum(item => item.UnitPrice * item.Quantity), 2),
                Math.Round(order.Items.Sum(item => item.UnitPrice * item.Quantity) + order.DeliveryPrice, 2),
                order.Status.ToString(),
                order.PaymentIntentId))
            .FirstOrDefaultAsync(ct);
    }
}
