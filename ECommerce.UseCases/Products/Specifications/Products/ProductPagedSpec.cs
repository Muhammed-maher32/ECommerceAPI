using Ardalis.Specification;
using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Enums;

namespace ECommerce.UseCases.Products.Specifications.Products;

public sealed class ProductPagedSpec : Specification<Product, GetAllProductsResponse>
{
    public ProductPagedSpec(
        string? search = null,
        Guid? brandId = null,
        Guid? typeId = null,
        ProductSortField? sortBy = null,
        bool sortDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            // Lowercased on both sides so the provider emits a case-insensitive
            // LIKE; a bare Contains maps to a case-sensitive LIKE on PostgreSQL.
            var term = search.Trim().ToLower();

            Query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term));
        }

        if (brandId.HasValue)
        {
            Query.Where(p => p.ProductBrandId == brandId.Value);
        }

        if (typeId.HasValue)
        {
            Query.Where(p => p.ProductTypeId == typeId.Value);
        }

        ApplySort(Query, sortBy, sortDescending);

        Query.Select(p => new GetAllProductsResponse(p.Id,
            p.Name, p.Description, p.Price, p.PictureUrl, p.ProductType.Name, p.ProductBrand.Name));

        //Pagination
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            Query.Skip(skip)
                .Take(pageSize.Value);
        }
    }

    private void ApplySort(ISpecificationBuilder<Product, GetAllProductsResponse> query,
        ProductSortField? sortBy, bool sortDescending)
    {
        switch (sortBy)
        {
            case ProductSortField.Name:
                if (sortDescending)
                    query.OrderByDescending(p => p.Name);
                else
                    query.OrderBy(p => p.Name);
                break;

            case ProductSortField.Price:
                if (sortDescending)
                    query.OrderByDescending(p => p.Price);
                else
                    query.OrderBy(p => p.Price);
                break;

            case ProductSortField.Brand:
                if (sortDescending)
                    query.OrderByDescending(p => p.ProductBrand.Name);
                else
                    query.OrderBy(p => p.ProductBrand.Name);
                break;

            case ProductSortField.Type:
                if (sortDescending)
                    query.OrderByDescending(p => p.ProductType.Name);
                else
                    query.OrderBy(p => p.ProductType.Name);
                break;

            default:
                query.OrderBy(p => p.Name);
                break;

        }
    }
}