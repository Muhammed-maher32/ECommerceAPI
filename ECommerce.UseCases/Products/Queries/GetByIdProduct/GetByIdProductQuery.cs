using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetByIdProduct;

public sealed record GetByIdProductQuery(Guid id) : IRequest<Result<GetByIdProductResponse>>;
