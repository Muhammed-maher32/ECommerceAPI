using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

public class DeliveryMethod : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 300;
    public const int MaxDeliveryTimeLength = 100;

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string DeliveryTime { get; private set; } = null!;
    public decimal Price { get; private set; }

    private DeliveryMethod() { }


    public static Result<DeliveryMethod> Create(
       Guid id,
       string name,
       string description,
       string deliveryTime,
       decimal price)
    {
        if (id == Guid.Empty)
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.IdRequired);

        if (string.IsNullOrWhiteSpace(name))
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.NameRequired);

        if (string.IsNullOrWhiteSpace(description))
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.DescriptionRequired);

        if (string.IsNullOrWhiteSpace(deliveryTime))
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.DeliveryTimeRequired);

        //Free shipping
        if (price < 0)
            return Result<DeliveryMethod>.Failure(DeliveryMethodErrors.PriceNegative);

        return Result<DeliveryMethod>.Success(new()
        {
            Id = id,
            Name = name,
            Description = description,
            DeliveryTime = deliveryTime,
            Price = price
        });
    }
}