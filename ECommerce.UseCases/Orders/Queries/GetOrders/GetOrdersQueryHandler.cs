using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderQueryService orderQueryService) :
    IRequestHandler<GetOrdersQuery, Result<IReadOnlyList<OrderSummaryResponse>>>
{
    public async Task<Result<IReadOnlyList<OrderSummaryResponse>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await orderQueryService.GetOrdersForBuyerAsync(
            request.BuyerId, cancellationToken);

        // An empty history is an empty list, not a 404.
        return Result<IReadOnlyList<OrderSummaryResponse>>.Success(orders);
    }
}
