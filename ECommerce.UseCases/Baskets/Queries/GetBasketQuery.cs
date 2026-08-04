using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Baskets.Dtos;
using Mapster;
using MediatR;

namespace ECommerce.UseCases.Baskets.Queries;

public record GetBasketQuery(Guid BuyerId) : IRequest<Result<CustomerBasketResponse>>;

public class GetBasketQueryHandler(IBasketRepository basketRepository) :
    IRequestHandler<GetBasketQuery, Result<CustomerBasketResponse>>
{
    public async Task<Result<CustomerBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BuyerId, cancellationToken);

        if (basket is null)
        {
            var createResult = Basket.CreateEmpty(request.BuyerId);
            if (createResult.IsFailure)
                return Result<CustomerBasketResponse>.Failure(createResult.Error!);

            basket = createResult.Value;
        }

        return Result<CustomerBasketResponse>.Success(basket.Adapt<CustomerBasketResponse>());
    }
}
