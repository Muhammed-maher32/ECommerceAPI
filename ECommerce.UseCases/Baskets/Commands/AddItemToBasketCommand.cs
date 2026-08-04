using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands;

public record AddItemToBasketCommand(
    Guid BuyerId,
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity) : IRequest<Result<CustomerBasketResponse>>;

public class AddItemToBasketCommandHandler(IBasketRepository basketRepository) :
    IRequestHandler<AddItemToBasketCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BuyerId, cancellationToken);

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

        await basketRepository.UpdateBasketAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
