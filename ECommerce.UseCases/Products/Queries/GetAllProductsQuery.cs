using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetAllProductsQuery() : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;

