using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.RemoveBasketItem;

public class RemoveItemFromBasketCommandHandler(IBasketStore basketStore) :
    IRequestHandler<RemoveItemFromBasketCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<CustomerBasketResponse>.Failure(BasketErrors.ItemNotFound);

        var removeResult = basket.RemoveItem(request.ProductId);

        if (removeResult.IsFailure)
            return Result<CustomerBasketResponse>.Failure(removeResult.Error!);

        await basketStore.SaveAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
