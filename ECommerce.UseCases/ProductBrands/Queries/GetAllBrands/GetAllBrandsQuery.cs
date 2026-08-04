using ECommerce.Domain.Shared;
using ECommerce.UseCases.ProductBrands.Dtos;
using MediatR;

namespace ECommerce.UseCases.ProductBrands.Queries;

public record GetAllBrandsQuery() : IRequest<Result<IReadOnlyList<GetAllBrandsResponse>>>;

