using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Brands;

public class BrandWithProductsSpec : Specification<ProductBrand>
{
    public BrandWithProductsSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(b => b.Products)
            .AsNoTracking();
    }
}
