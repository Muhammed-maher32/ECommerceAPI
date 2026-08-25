using ECommerce.Domain.Shared;
using ECommerce.UseCases.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.DeliveryMethods.Queries.GetAllDeliveryMethods;

public record GetAllDeliveryMethodsQuery()
    : IRequest<Result<IReadOnlyList<GetAllDeliveryMethodsResponse>>>;

