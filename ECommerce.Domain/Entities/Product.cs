using ECommerce.Domain.Shared;

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

    private Product() { }

    public static Result<Product> Create(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        var product = new Product();

        var result = product.SetName(name);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        result = product.SetDescription(description);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        result = product.SetPictureUrl(pictureUrl);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        result = product.SetPrice(price);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        result = product.SetBrand(productBrandId);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        result = product.SetType(productTypeId);
        if (result.IsFailure)
            return Result<Product>.Failure(result.Error!);

        return Result<Product>.Success(product);
    }

    public Result Update(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        var result = SetName(name);
        if (result.IsFailure)
            return result;

        result = SetDescription(description);
        if (result.IsFailure)
            return result;

        result = SetPictureUrl(pictureUrl);
        if (result.IsFailure)
            return result;

        result = SetPrice(price);
        if (result.IsFailure)
            return result;

        result = SetBrand(productBrandId);
        if (result.IsFailure)
            return result;

        result = SetType(productTypeId);
        if (result.IsFailure)
            return result;

        //UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success();
    }

    private Result SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(ProductErrors.NameRequired);

        name = name.Trim();

        if (name.Length > MaxNameLength)
            return Result.Failure(ProductErrors.NameTooLong);

        Name = name;

        return Result.Success();
    }

    private Result SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(ProductErrors.DescriptionRequired);

        description = description.Trim();

        if (description.Length > MaxDescriptionLength)
            return Result.Failure(ProductErrors.DescriptionTooLong);

        Description = description;

        return Result.Success();
    }

    private Result SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result.Failure(ProductErrors.PictureUrlRequired);

        pictureUrl = pictureUrl.Trim();

        if (pictureUrl.Length > MaxPictureUrlLength)
            return Result.Failure(ProductErrors.PictureUrlTooLong);

        PictureUrl = pictureUrl;

        return Result.Success();
    }

    private Result SetPrice(decimal price)
    {
        if (price <= 0)
            return Result.Failure(ProductErrors.InvalidPrice);

        Price = price;

        return Result.Success();
    }

    private Result SetBrand(Guid productBrandId)
    {
        if (productBrandId == Guid.Empty)
            return Result.Failure(ProductErrors.BrandRequired);

        ProductBrandId = productBrandId;

        return Result.Success();
    }

    private Result SetType(Guid productTypeId)
    {
        if (productTypeId == Guid.Empty)
            return Result.Failure(ProductErrors.TypeRequired);

        ProductTypeId = productTypeId;

        return Result.Success();
    }
}