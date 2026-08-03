using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.UseCases.Common.Services;
using Microsoft.Extensions.Options;
using ModelImageUploadResult = ECommerce.UseCases.Common.Models.ImageUploadResult;

namespace ECommerce.Infrastructure.Services;

public class CloudinaryPhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryPhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ModelImageUploadResult?> UploadPhotoAsync(Stream fileStream,
        string fileName, CancellationToken ct = default)
    {
        if (fileStream.Length == 0) return null;

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
            Folder = "ecommerce-products"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams, ct);

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        //upload result returns SecureUrl (ends .jpg)
        //also returns public Id
        return new ModelImageUploadResult(
            uploadResult.PublicId,
            uploadResult.SecureUrl.ToString()
        );
    }

    public async Task<bool> DeletePhotoAsync(string publicId, CancellationToken ct = default)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        return result.Result == "ok";
    }
}
