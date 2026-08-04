using ECommerce.Domain.Shared;
using ECommerce.UseCases.ProductTypes.Dtos;
using MediatR;

namespace ECommerce.UseCases.ProductTypes.Queries;

public record GetAllTypesQuery() : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;
