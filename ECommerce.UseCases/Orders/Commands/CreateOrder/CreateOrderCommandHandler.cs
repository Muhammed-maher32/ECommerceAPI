using ECommerce.Domain.Entities;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using ECommerce.UseCases.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.UseCases.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IBasketStore basketStore,
    IProductQueryService productQueryService,
    IUnitOfWork unitOfWork,
    ILogger<CreateOrderCommandHandler> logger) :
    IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<OrderResponse>.Failure(BasketErrors.BasketNotFound);

        if (basket.Items.Count == 0)
            return Result<OrderResponse>.Failure(OrderErrors.EmptyOrder);

        // Everything below is re-read from the database. The basket lives in Redis and
        // its prices are a snapshot the client could also have influenced; only the
        // catalogue decides what an order costs.
        var productIds = basket.Items
            .Select(item => item.ProductId)
            .Distinct()
            .ToList();

        var snapshots = await productQueryService.GetPricingSnapshotsAsync(
            productIds, cancellationToken);

        if (snapshots.Count != productIds.Count)
            return Result<OrderResponse>.Failure(ProductErrors.NotFound);

        var deliveryMethod = await unitOfWork
            .Repository<DeliveryMethod>()
            .GetByIdAsync(request.DeliveryMethodId, cancellationToken);

        if (deliveryMethod is null)
            return Result<OrderResponse>.Failure(DeliveryMethodErrors.NotFound);

        var addressResult = ShippingAddress.Create(
            request.ShipToAddress.RecipientFirstName,
            request.ShipToAddress.RecipientLastName,
            request.ShipToAddress.PhoneNumber,
            request.ShipToAddress.Country,
            request.ShipToAddress.City,
            request.ShipToAddress.Street,
            request.ShipToAddress.PostalCode);

        if (addressResult.IsFailure)
            return Result<OrderResponse>.Failure(addressResult.Error!);

        var items = new List<OrderItem>(basket.Items.Count);

        foreach (var basketItem in basket.Items)
        {
            var product = snapshots[basketItem.ProductId];

            var itemResult = OrderItem.Create(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price,
                basketItem.Quantity);

            if (itemResult.IsFailure)
                return Result<OrderResponse>.Failure(itemResult.Error!);

            items.Add(itemResult.Value);
        }

        var orderResult = Order.Create(
            request.BuyerId,
            request.BuyerEmail,
            addressResult.Value,
            deliveryMethod.Name,
            deliveryMethod.Price,
            items);

        if (orderResult.IsFailure)
            return Result<OrderResponse>.Failure(orderResult.Error!);

        var order = orderResult.Value;

        unitOfWork.Repository<Order>().Add(order);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Only after the order is committed. Clearing first would lose the basket if
        // the insert failed; a basket that outlives a successful order is recoverable,
        // a basket lost with no order to show for it is not.
        try
        {
            await basketStore.DeleteAsync(request.BuyerId, cancellationToken);
        }
        catch (Exception ex)
        {
            // The order exists and is the caller's answer. A stale basket is a nuisance,
            // not a reason to report failure for work that already succeeded.
            logger.LogError(ex,
                "Order {OrderId} was created but its basket {BuyerId} could not be cleared.",
                order.Id, request.BuyerId);
        }

        return Result<OrderResponse>.Success(ToResponse(order));
    }

    private static OrderResponse ToResponse(Order order) =>
        new(
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
            [.. order.Items.Select(item => new OrderItemResponse(
                item.ProductId,
                item.ProductName,
                item.PictureUrl,
                item.UnitPrice,
                item.Quantity,
                item.LineTotal))],
            order.Subtotal,
            order.Total,
            order.Status.ToString(),
            order.PaymentIntentId);
}
