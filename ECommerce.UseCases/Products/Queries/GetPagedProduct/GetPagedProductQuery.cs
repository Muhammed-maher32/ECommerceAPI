using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Enums;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetPagedProductQuery(
    int PageNumber = 1,
    int PageSize = 5,
    string? Search = null,
    Guid? BrandId = null,
    Guid? TypeId = null,

    //Enable sorting

    ProductSortField? SortBy = ProductSortField.Name,
    bool SortDescending = false

    ) : IRequest<Result<PagedResult<GetAllProductsResponse>>>;