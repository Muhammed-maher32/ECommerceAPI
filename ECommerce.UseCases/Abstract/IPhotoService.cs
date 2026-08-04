namespace ECommerce.UseCases.Abstract;

public interface IPhotoService
{
    Task<ImageUploadResult?> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    Task<bool> DeletePhotoAsync(string publicId, CancellationToken ct = default);
}
