using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

/// <summary>
/// A snapshot of a product as it was when the order was placed. The catalogue
/// can rename, re-picture, re-price, or soft-delete the product afterwards;
/// none of that may change what the buyer agreed to pay.
/// </summary>
public sealed class OrderItem : BaseEntity
{
    public const int MinQuantity = 1;
    public const int MaxQuantity = 99;
    public const int MaxProductNameLength = 100;
    public const int MaxPictureUrlLength = 500;

    private OrderItem() { }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal LineTotal => UnitPrice * Quantity;

    public static Result<OrderItem> Create(
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        if (productId == Guid.Empty)
            return Result<OrderItem>.Failure(OrderErrors.InvalidProductId);

        if (string.IsNullOrWhiteSpace(productName))
            return Result<OrderItem>.Failure(OrderErrors.InvalidProductName);

        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result<OrderItem>.Failure(OrderErrors.InvalidPictureUrl);

        if (unitPrice <= 0)
            return Result<OrderItem>.Failure(OrderErrors.InvalidUnitPrice);

        if (quantity < MinQuantity || quantity > MaxQuantity)
            return Result<OrderItem>.Failure(OrderErrors.InvalidQuantity);

        return Result<OrderItem>.Success(new OrderItem
        {
            ProductId = productId,
            ProductName = productName.Trim(),
            PictureUrl = pictureUrl.Trim(),
            UnitPrice = unitPrice,
            Quantity = quantity
        });
    }
}
