using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.RemoveBasketItem;

public record RemoveItemFromBasketCommand(
    Guid BuyerId,
    Guid ProductId) : IRequest<Result<CustomerBasketResponse>>;
