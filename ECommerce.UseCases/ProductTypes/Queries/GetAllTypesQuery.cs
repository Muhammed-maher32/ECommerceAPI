using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductTypes.Dtos;
using MediatR;

namespace ECommerce.UseCases.ProductTypes.Queries;

public record GetAllTypesQuery() : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;
