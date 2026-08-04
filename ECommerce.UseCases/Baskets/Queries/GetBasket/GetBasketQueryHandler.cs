using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Queries.GetBasket;

public class GetBasketQueryHandler(IBasketStore basketStore) :
    IRequestHandler<GetBasketQuery, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<CustomerBasketResponse>.Failure(BasketErrors.BasketNotFound);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
