using ECommerce.Domain.Shared;
using ECommerce.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.UseCases.Orders.Commands.CreateOrder;

/// <summary>
/// BuyerId and BuyerEmail come from the access token, never from the body.
/// There are no prices and no line items here by design.
/// </summary>
public record CreateOrderCommand(
    Guid BuyerId,
    string BuyerEmail,
    ShippingAddressRequest ShipToAddress,
    Guid DeliveryMethodId) : IRequest<Result<OrderResponse>>;
