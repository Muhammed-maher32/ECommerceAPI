using ECommerce.Domain.Common;
using ECommerce.UseCases.Types.Dtos;
using MediatR;

namespace ECommerce.UseCases.Types.Queries.GetAllTypes;

public record GetAllTypesQuery() : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;
