using ECommerce.Domain.Common;
using ECommerce.UseCases.Prdoucts.Dtos;
using MediatR;

namespace ECommerce.UseCases.Prdoucts.Queries.GetByIdProduct;

public record GetByIdProductQuery(Guid id) : IRequest<Result<GetByIdProductResponse>>;
