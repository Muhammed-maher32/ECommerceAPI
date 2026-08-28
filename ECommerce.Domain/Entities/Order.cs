using ECommerce.Domain.Enums;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

public sealed class Order : BaseEntity
{
    public const int MaxEmailLength = 256;
    public const int MaxDeliveryMethodNameLength = 100;
    public const int MaxPaymentIntentIdLength = 128;

    private readonly List<OrderItem> _items = [];

    private Order() { }

    public Guid BuyerId { get; private set; }
    public string BuyerEmail { get; private set; } = null!;
    public DateTimeOffset OrderDate { get; private set; }

    public ShippingAddress ShipToAddress { get; private set; } = null!;

    public string DeliveryMethodName { get; private set; } = null!;
    public decimal DeliveryPrice { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal Subtotal => _items.Sum(item => item.LineTotal);
    public decimal Total => Subtotal + DeliveryPrice;

    public OrderStatus Status { get; private set; }
    public string? PaymentIntentId { get; private set; }

    public static Result<Order> Create(
        Guid buyerId,
        string buyerEmail,
        ShippingAddress shipToAddress,
        string deliveryMethodName,
        decimal deliveryPrice,
        IEnumerable<OrderItem> items,
        string? paymentIntentId = null)
    {
        if (buyerId == Guid.Empty)
            return Result<Order>.Failure(OrderErrors.InvalidBuyerId);

        if (string.IsNullOrWhiteSpace(buyerEmail))
            return Result<Order>.Failure(OrderErrors.InvalidBuyerEmail);

        if (shipToAddress is null)
            return Result<Order>.Failure(OrderErrors.ShippingAddressRequired);

        if (string.IsNullOrWhiteSpace(deliveryMethodName))
            return Result<Order>.Failure(OrderErrors.InvalidDeliveryMethod);

        // Zero is allowed: free shipping, same rule as DeliveryMethod.
        if (deliveryPrice < 0)
            return Result<Order>.Failure(OrderErrors.InvalidDeliveryPrice);

        var orderItems = items?.ToList() ?? [];

        if (orderItems.Count == 0)
            return Result<Order>.Failure(OrderErrors.EmptyOrder);

        if (orderItems.Select(i => i.ProductId).Distinct().Count() != orderItems.Count)
            return Result<Order>.Failure(OrderErrors.DuplicateOrderItem);

        var order = new Order
        {
            BuyerId = buyerId,
            BuyerEmail = buyerEmail.Trim(),
            OrderDate = DateTimeOffset.UtcNow,
            ShipToAddress = shipToAddress,
            DeliveryMethodName = deliveryMethodName.Trim(),
            DeliveryPrice = deliveryPrice,
            Status = OrderStatus.Pending,
            PaymentIntentId = string.IsNullOrWhiteSpace(paymentIntentId)
                ? null
                : paymentIntentId.Trim()
        };

        order._items.AddRange(orderItems);

        return Result<Order>.Success(order);
    }

    /// <summary>
    /// Attaches the payment intent. Set once, so a webhook carrying an unrelated
    /// intent can never be reconciled against this order.
    /// </summary>
    public Result SetPaymentIntent(string paymentIntentId)
    {
        if (string.IsNullOrWhiteSpace(paymentIntentId))
            return Result.Failure(OrderErrors.InvalidPaymentIntentId);

        var trimmed = paymentIntentId.Trim();

        if (PaymentIntentId is not null && PaymentIntentId != trimmed)
            return Result.Failure(OrderErrors.PaymentIntentAlreadySet);

        PaymentIntentId = trimmed;

        return Result.Success();
    }

    public Result MarkAsPaid()
    {
        // Payment webhooks are delivered more than once, so re-applying the same
        // transition is a success, not a conflict.
        if (Status == OrderStatus.PaymentReceived)
            return Result.Success();

        // A failed attempt can be retried, so PaymentFailed -> PaymentReceived is legal.
        if (Status is not (OrderStatus.Pending or OrderStatus.PaymentFailed))
            return Result.Failure(OrderErrors.CannotMarkAsPaid(Status));

        if (string.IsNullOrWhiteSpace(PaymentIntentId))
            return Result.Failure(OrderErrors.PaymentIntentRequired);

        Status = OrderStatus.PaymentReceived;

        return Result.Success();
    }

    public Result MarkAsFailed()
    {
        if (Status == OrderStatus.PaymentFailed)
            return Result.Success();

        if (Status is not OrderStatus.Pending)
            return Result.Failure(OrderErrors.CannotMarkAsFailed(Status));

        Status = OrderStatus.PaymentFailed;

        return Result.Success();
    }

    public Result MarkAsShipped()
    {
        if (Status is not OrderStatus.PaymentReceived)
            return Result.Failure(OrderErrors.CannotShip(Status));

        Status = OrderStatus.Shipped;

        return Result.Success();
    }

    public Result MarkAsDelivered()
    {
        if (Status is not OrderStatus.Shipped)
            return Result.Failure(OrderErrors.CannotDeliver(Status));

        Status = OrderStatus.Delivered;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            return Result.Success();

        // Once it is on a van it is a return, not a cancellation.
        if (Status is not (OrderStatus.Pending or OrderStatus.PaymentFailed))
            return Result.Failure(OrderErrors.CannotCancel(Status));

        Status = OrderStatus.Cancelled;

        return Result.Success();
    }
}
