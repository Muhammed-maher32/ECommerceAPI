using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using ECommerce.UseCases.Products;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.AddBasketItem;

public class AddItemToBasketCommandHandler(
    IBasketStore basketStore,
    IProductQueryService productQueryService) :
    IRequestHandler<AddItemToBasketCommand, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(AddItemToBasketCommand request,
        CancellationToken cancellationToken)
    {
        var product = await productQueryService.GetByIdProductAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Result<CustomerBasketResponse>.Failure(ProductErrors.NotFound);

        var result = await basketStore.MutateAsync(
            request.BuyerId,
            basket => basket.AddItem(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price,
                request.Quantity),
            createIfMissing: true,
            cancellationToken);

        return result.IsFailure
            ? Result<CustomerBasketResponse>.Failure(result.Error!)
            : Result<CustomerBasketResponse>.Success(result.Value.Adapt<CustomerBasketResponse>());
    }
}
