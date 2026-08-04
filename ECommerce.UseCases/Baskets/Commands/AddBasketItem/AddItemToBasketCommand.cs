using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.AddBasketItem;

public record AddItemToBasketCommand(
    Guid BuyerId,
    Guid ProductId,
    int Quantity) : IRequest<Result<CustomerBasketResponse>>;
