using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries;

public sealed record GetByIdProductQuery(Guid id) : IRequest<Result<GetByIdProductResponse>>;
