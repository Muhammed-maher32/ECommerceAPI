using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.AddBasketItem;

public class AddItemToBasketCommandHandler(IBasketStore basketStore) :
    IRequestHandler<AddItemToBasketCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketStore.GetAsync(request.BuyerId, cancellationToken);

        if (basket is null)
        {
            var createResult = Basket.CreateEmpty(request.BuyerId);
            if (createResult.IsFailure)
                return Result<CustomerBasketResponse>.Failure(createResult.Error!);

            basket = createResult.Value;
        }

        var addResult = basket.AddItem(
            request.ProductId,
            request.ProductName,
            request.PictureUrl,
            request.UnitPrice,
            request.Quantity);

        if (addResult.IsFailure)
            return Result<CustomerBasketResponse>.Failure(addResult.Error!);

        await basketStore.SaveAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
