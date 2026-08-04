using ECommerce.Domain.Shared;
using ECommerce.UseCases.Baskets.Dtos;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.UpdateBasketItemQuantity;

public record UpdateBasketItemQuantityCommand(
    Guid BuyerId,
    Guid ProductId,
    int Quantity) : IRequest<Result<CustomerBasketResponse>>;
