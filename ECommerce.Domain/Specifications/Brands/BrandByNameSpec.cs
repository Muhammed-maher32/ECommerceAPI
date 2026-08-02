using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Brands;

public sealed class BrandByNameSpec : Specification<ProductBrand>
{
    public BrandByNameSpec(string name)
    {
        Query
            .Where(b => b.Name == name)
            .AsNoTracking();
    }
}