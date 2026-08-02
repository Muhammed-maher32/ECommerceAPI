using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetByIdProduct;

public record GetByIdProductQuery(Guid id) : IRequest<Result<GetByIdProductResponse>>;
