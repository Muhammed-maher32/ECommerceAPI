using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using MediatR;

namespace ECommerce.UseCases.Baskets.Queries.GetBasket;

public record GetBasketQuery(Guid BuyerId) : IRequest<Result<CustomerBasketResponse>>;
