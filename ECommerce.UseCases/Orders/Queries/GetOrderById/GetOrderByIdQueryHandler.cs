using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IOrderQueryService orderQueryService) :
    IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await orderQueryService.GetOrderForBuyerAsync(
            request.OrderId, request.BuyerId, cancellationToken);

        // Someone else's order and a non-existent order are the same answer on purpose.
        // A 403 here would confirm the id is real and leak how many orders the shop has.
        return order is null
            ? Result<OrderResponse>.Failure(OrderErrors.NotFound)
            : Result<OrderResponse>.Success(order);
    }
}
