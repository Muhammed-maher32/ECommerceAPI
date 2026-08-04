using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

public sealed class ProductType : BaseEntity
{
    public string Name { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];

    private ProductType() { }
    public static Result<ProductType> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<ProductType>.Failure(ProductTypeErrors.IdRequired);

        if (string.IsNullOrWhiteSpace(name))
            return Result<ProductType>.Failure(ProductTypeErrors.NameRequired);

        return Result<ProductType>.Success(new()
        {
            Id = id,
            Name = name.Trim(),
        });
    }
}
