namespace ECommerce.UseCases.Products.Dtos;

/// <summary>
/// The catalogue fields an order line is built from, read fresh at checkout.
/// The basket's copies are stale by definition -- it can outlive a price change.
/// </summary>
public record ProductPricingSnapshot(
    Guid Id,
    string Name,
    string PictureUrl,
    decimal Price);
