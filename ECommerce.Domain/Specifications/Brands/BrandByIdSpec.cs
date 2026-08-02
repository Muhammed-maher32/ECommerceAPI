using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Brands;

public sealed class BrandByIdSpec : Specification<ProductBrand>
{
    public BrandByIdSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id)
            .AsNoTracking();
    }
}
