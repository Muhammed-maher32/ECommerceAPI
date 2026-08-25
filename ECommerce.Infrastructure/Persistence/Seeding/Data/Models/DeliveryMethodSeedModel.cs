namespace ECommerce.Infrastructure.Persistence.Seeding.Data.Models;

public record DeliveryMethodSeedModel(
    Guid Id,
    string Name,
    string Description,
    string DeliveryTime,
    decimal Price
    );

