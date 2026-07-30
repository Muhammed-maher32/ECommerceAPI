namespace ECommerce.Domain.Entities;

public sealed class Product : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 1000;
    public const int MaxPictureUrlLength = 500;

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal Price { get; private set; }

    public Guid ProductBrandId { get; private set; }
    public ProductBrand ProductBrand { get; private set; } = null!;

    public Guid ProductTypeId { get; private set; }
    public ProductType ProductType { get; private set; } = null!;

    private Product()
    {
    }

    public Product(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        SetName(name);
        SetDescription(description);
        SetPictureUrl(pictureUrl);
        SetPrice(price);
        SetBrand(productBrandId);
        SetType(productTypeId);
    }

    public void Update(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        SetName(name);
        SetDescription(description);
        SetPictureUrl(pictureUrl);
        SetPrice(price);
        SetBrand(productBrandId);
        SetType(productTypeId);
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        name = name.Trim();

        if (name.Length > MaxNameLength)
            throw new ArgumentException(
                $"Product name cannot exceed {MaxNameLength} characters.",
                nameof(name));

        Name = name;
    }

    private void SetDescription(string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        description = description.Trim();

        if (description.Length > MaxDescriptionLength)
            throw new ArgumentException(
                $"Description cannot exceed {MaxDescriptionLength} characters.",
                nameof(description));

        Description = description;
    }

    private void SetPictureUrl(string pictureUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pictureUrl);

        pictureUrl = pictureUrl.Trim();

        if (pictureUrl.Length > MaxPictureUrlLength)
            throw new ArgumentException(
                $"Picture URL cannot exceed {MaxPictureUrlLength} characters.",
                nameof(pictureUrl));

        PictureUrl = pictureUrl;
    }

    private void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price));

        Price = price;
    }

    private void SetBrand(Guid productBrandId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(productBrandId, Guid.Empty);

        ProductBrandId = productBrandId;
    }

    private void SetType(Guid productTypeId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(productTypeId, Guid.Empty);

        ProductTypeId = productTypeId;
    }
}