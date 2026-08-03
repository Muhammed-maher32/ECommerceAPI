using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Enums;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetPagedProductQuery(
    int pageNumber = 1,
    int pageSize = 5,
    string? search = null,
    Guid? BrandId = null,
    Guid? TypeId = null,

    //Enable sorting

    ProductSortField? SortBy = ProductSortField.Name,
    bool SortDescending = false

    ) : IRequest<Result<PagedResult<GetAllProductsResponse>>>;
