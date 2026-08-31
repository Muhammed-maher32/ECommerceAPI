using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries.GetOrders;

public record GetOrdersQuery(Guid BuyerId)
    : IRequest<Result<IReadOnlyList<OrderSummaryResponse>>>;
