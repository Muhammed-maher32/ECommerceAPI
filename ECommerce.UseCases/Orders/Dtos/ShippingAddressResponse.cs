namespace ECommerce.UseCases.Orders.Dtos;

public record ShippingAddressResponse(
    string RecipientFirstName,
    string RecipientLastName,
    string PhoneNumber,
    string Country,
    string City,
    string Street,
    string PostalCode);
