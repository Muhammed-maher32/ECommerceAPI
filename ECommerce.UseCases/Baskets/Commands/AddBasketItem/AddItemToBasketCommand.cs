using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.AddBasketItem;

public record AddItemToBasketCommand(
    Guid BuyerId,
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity) : IRequest<Result<CustomerBasketResponse>>;
