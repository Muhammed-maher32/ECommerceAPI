using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.ClearBasket;

public class DeleteBasketCommandHandler(IBasketStore basketStore) :
    IRequestHandler<DeleteBasketCommand, Result>
{
    public async Task<Result> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await basketStore.DeleteAsync(request.BuyerId, cancellationToken);
        return Result.Success();
    }
}
