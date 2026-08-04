using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.UpdateBasketItemQuantity;

public class UpdateBasketItemQuantityCommandHandler(IBasketStore basketStore) :
    IRequestHandler<UpdateBasketItemQuantityCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<CustomerBasketResponse>.Failure(BasketErrors.ItemNotFound);

        var updateResult = basket.UpdateItemQuantity(request.ProductId, request.Quantity);

        if (updateResult.IsFailure)
            return Result<CustomerBasketResponse>.Failure(updateResult.Error!);

        await basketStore.SaveAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
