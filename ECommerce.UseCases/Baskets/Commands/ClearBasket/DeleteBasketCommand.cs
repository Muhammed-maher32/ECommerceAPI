using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Baskets.Commands.ClearBasket;

public record DeleteBasketCommand(Guid BuyerId) : IRequest<Result>;
