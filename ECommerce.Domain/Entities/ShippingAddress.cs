using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities;

/// <summary>
/// A snapshot of where the order is shipped. Deliberately a copy of the
/// <see cref="UserAddress"/> chosen at checkout, never a reference to it:
/// editing or deleting the address book entry must not rewrite history.
/// </summary>
public sealed class ShippingAddress
{
    public const int MaxNameLength = 100;
    public const int MaxPhoneLength = 32;
    public const int MaxCountryLength = 100;
    public const int MaxCityLength = 100;
    public const int MaxStreetLength = 200;
    public const int MaxPostalCodeLength = 20;

    private ShippingAddress() { }

    public string RecipientFirstName { get; private set; } = null!;
    public string RecipientLastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;

    public static Result<ShippingAddress> Create(
        string recipientFirstName,
        string recipientLastName,
        string phoneNumber,
        string country,
        string city,
        string street,
        string postalCode)
    {
        if (string.IsNullOrWhiteSpace(recipientFirstName) ||
            string.IsNullOrWhiteSpace(recipientLastName))
            return Result<ShippingAddress>.Failure(OrderErrors.InvalidRecipientName);

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result<ShippingAddress>.Failure(OrderErrors.InvalidPhone);

        if (string.IsNullOrWhiteSpace(country) ||
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(street))
            return Result<ShippingAddress>.Failure(OrderErrors.InvalidLocation);

        if (string.IsNullOrWhiteSpace(postalCode))
            return Result<ShippingAddress>.Failure(OrderErrors.InvalidPostalCode);

        return Result<ShippingAddress>.Success(new ShippingAddress
        {
            RecipientFirstName = recipientFirstName.Trim(),
            RecipientLastName = recipientLastName.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            Country = country.Trim(),
            City = city.Trim(),
            Street = street.Trim(),
            PostalCode = postalCode.Trim()
        });
    }

    public static Result<ShippingAddress> FromUserAddress(UserAddress address)
        => Create(
            address.RecipientFirstName,
            address.RecipientLastName,
            address.PhoneNumber,
            address.Country,
            address.City,
            address.Street,
            address.PostalCode);
}
