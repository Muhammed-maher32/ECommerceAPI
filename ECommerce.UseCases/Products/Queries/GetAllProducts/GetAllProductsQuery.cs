using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetAllProducts;

public record GetAllProductsQuery() : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;

