using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Types;

public sealed class TypeByIdSpec : Specification<ProductType>
{
    public TypeByIdSpec(Guid id)
    {
        Query
            .Where(t => t.Id == id)
            .AsNoTracking();
    }
}