namespace ECommerce.UseCases.Orders.Dtos;

public record ShippingAddressRequest(
    string RecipientFirstName,
    string RecipientLastName,
    string PhoneNumber,
    string Country,
    string City,
    string Street,
    string PostalCode);
