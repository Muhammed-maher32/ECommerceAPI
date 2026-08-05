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
        var result = await basketStore.MutateAsync(
            request.BuyerId,
            basket => basket.UpdateItemQuantity(request.ProductId, request.Quantity),
            createIfMissing: false,
            cancellationToken);

        return result.IsFailure
            ? Result<CustomerBasketResponse>.Failure(result.Error!)
            : Result<CustomerBasketResponse>.Success(result.Value.Adapt<CustomerBasketResponse>());
    }
}
