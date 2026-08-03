using ECommerce.UseCases.Common.Models;

namespace ECommerce.UseCases.Common.Services;

public interface IPhotoService
{
    Task<ImageUploadResult?> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    Task<bool> DeletePhotoAsync(string publicId, CancellationToken ct = default);
}
