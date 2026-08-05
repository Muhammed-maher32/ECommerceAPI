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
        var result = await basketStore.MutateAsync(
            request.BuyerId,
            basket => basket.RemoveItem(request.ProductId),
            createIfMissing: false,
            cancellationToken);

        return result.IsFailure
            ? Result<CustomerBasketResponse>.Failure(result.Error!)
            : Result<CustomerBasketResponse>.Success(result.Value.Adapt<CustomerBasketResponse>());
    }
}
