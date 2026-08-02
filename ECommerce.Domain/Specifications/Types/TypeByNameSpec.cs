using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Types;

public sealed class TypeByNameSpec : Specification<ProductType>
{
    public TypeByNameSpec(string name)
    {
        Query
            .Where(t => t.Name == name)
            .AsNoTracking();
    }
}