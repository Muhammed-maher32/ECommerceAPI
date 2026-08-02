using Ardalis.Specification;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Specifications.Products;

public sealed class ProductsFilterSpec : Specification<Product>
{
    public ProductsFilterSpec(Guid? brandId, Guid? typeId, int pageIndex, int pageSize)
    {
        Query
            .Where(p => !brandId.HasValue || p.ProductBrandId == brandId)
            .Where(p => !typeId.HasValue || p.ProductTypeId == typeId)
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .OrderBy(p => p.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}
