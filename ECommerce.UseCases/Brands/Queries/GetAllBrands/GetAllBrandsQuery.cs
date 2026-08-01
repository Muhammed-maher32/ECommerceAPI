using ECommerce.Domain.Common;
using ECommerce.UseCases.Brands.Dtos;
using MediatR;

namespace ECommerce.UseCases.Brands.Queries.GetAllBrands;

public record GetAllBrandsQuery() : IRequest<Result<IReadOnlyList<GetAllBrandsResponse>>>;

