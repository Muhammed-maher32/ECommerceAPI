namespace ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

public record ProductSeedModel(
    string Name,
    string Description,
    string PictureUrl,
    decimal Price,
    Guid ProductTypeId,
    Guid ProductBrandId);

