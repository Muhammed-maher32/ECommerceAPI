using ECommerce.Domain.Common;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands;

public record UpdateBasketItemQuantityCommand(
    Guid BuyerId,
    Guid ProductId,
    int Quantity) : IRequest<Result<CustomerBasketResponse>>;

public class UpdateBasketItemQuantityCommandHandler(IBasketRepository basketRepository) :
    IRequestHandler<UpdateBasketItemQuantityCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BuyerId, cancellationToken);

        if (basket is null)
            return Result<CustomerBasketResponse>.Failure(BasketErrors.ItemNotFound);

        var updateResult = basket.UpdateItemQuantity(request.ProductId, request.Quantity);

        if (updateResult.IsFailure)
            return Result<CustomerBasketResponse>.Failure(updateResult.Error!);

        await basketRepository.UpdateBasketAsync(basket, ct: cancellationToken);

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
