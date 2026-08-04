using ECommerce.Domain.Common;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands;

public record RemoveItemFromBasketCommand(
    Guid BuyerId,
    Guid ProductId) : IRequest<Result<CustomerBasketResponse>>;

public class RemoveItemFromBasketCommandHandler(IBasketRepository basketRepository) :
    IRequestHandler<RemoveItemFromBasketCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<CustomerBasketResponse>.Failure(BasketErrors.ItemNotFound);

        var removeResult = basket.RemoveItem(request.ProductId);

        if (removeResult.IsFailure)
            return Result<CustomerBasketResponse>.Failure(removeResult.Error!);

        await basketRepository.UpdateBasketAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
