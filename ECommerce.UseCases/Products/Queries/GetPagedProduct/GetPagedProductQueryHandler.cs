using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetPagedProduct;

public sealed class GetPagedProductQueryHandler(IReadRepository<Product> repository) :
    IRequestHandler<GetPagedProductQuery, Result<PagedResult<GetAllProductsResponse>>>
{
    public async Task<Result<PagedResult<GetAllProductsResponse>>> Handle(GetPagedProductQuery request,
        CancellationToken cancellationToken)
    {
        var countSpec = new ProductPagedSpecification
            (
            request.Search,
            request.BrandId,
            request.TypeId
            );

        var listSpecification = new ProductPagedSpecification
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
