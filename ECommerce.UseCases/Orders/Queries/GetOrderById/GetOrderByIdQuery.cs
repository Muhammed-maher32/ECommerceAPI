using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId, Guid BuyerId)
    : IRequest<Result<OrderResponse>>;
