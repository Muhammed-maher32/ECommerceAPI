using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Types;

public sealed class TypeWithProductsSpec : Specification<ProductType>
{
    public TypeWithProductsSpec(Guid id)
    {
        Query
            .Where(t => t.Id == id)
            .Include(t => t.Products)
            .AsNoTracking();
    }
}