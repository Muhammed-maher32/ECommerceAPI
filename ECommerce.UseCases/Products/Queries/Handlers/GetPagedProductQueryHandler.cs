using Ardalis.Specification;
using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.Handlers;

public sealed class GetPagedProductQueryHandler(IRepository<Product> repository) :
    IRequestHandler<GetPagedProductQuery, Result<PagedResult<GetAllProductsResponse>>>
{
    public async Task<Result<PagedResult<GetAllProductsResponse>>> Handle(GetPagedProductQuery request,
        CancellationToken cancellationToken)
    {
        var countSpec = new ProductPagedSpec
            (
            request.Search,
            request.BrandId,
            request.TypeId
            );

        var listSpecification = new ProductPagedSpec
            (
            search: request.Search,
            brandId: request.BrandId,
            typeId: request.TypeId,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
            );
        // pagination
        var items = await repository.ListAsync(listSpecification, cancellationToken);
        // count
        var totalCount = await repository.CountAsync(countSpec, cancellationToken);

        return Result<PagedResult<GetAllProductsResponse>>
            .Success(new PagedResult<GetAllProductsResponse>(items, totalCount));
    }
}
