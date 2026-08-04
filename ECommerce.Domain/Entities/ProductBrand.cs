using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

public sealed class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    private ProductBrand() { }

    public static Result<ProductBrand> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<ProductBrand>.Failure(ProductBrandErrors.IdRequired);

        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductBrand>.Failure(ProductBrandErrors.NameRequired);

        return Result<ProductBrand>.Success(new()
        {
            Id = id,
            Name = name.Trim()
        });
    }
}
