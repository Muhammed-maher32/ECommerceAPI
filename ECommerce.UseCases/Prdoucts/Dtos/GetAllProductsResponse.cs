namespace ECommerce.UseCases.Prdoucts.Dtos;

public record GetAllProductsResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    string ProductType,
    string ProductBrand
);


