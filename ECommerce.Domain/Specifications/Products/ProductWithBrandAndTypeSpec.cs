using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Products;

public sealed class ProductWithBrandAndTypeSpec : Specification<Product>
{
    public ProductWithBrandAndTypeSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.ProductBrandId)
            .Include(p => p.ProductTypeId)
            .AsNoTracking();
    }
}
