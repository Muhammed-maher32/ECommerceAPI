using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;
using MediatR;

namespace ECommerce.UseCases.Prdoucts.Queries.GetAllProducts;

public record GetAllProductsQuery() : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;

