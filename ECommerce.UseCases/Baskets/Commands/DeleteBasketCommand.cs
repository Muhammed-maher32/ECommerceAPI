using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands;

public record DeleteBasketCommand(Guid BuyerId) : IRequest<Result>;

public class DeleteBasketCommandHandler(IBasketRepository basketRepository) :
    IRequestHandler<DeleteBasketCommand, Result>
{
    public async Task<Result> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await basketRepository.DeleteBasketAsync(request.BuyerId, cancellationToken);
        return Result.Success();
    }
}
