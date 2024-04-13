using CloudinaryDotNet.Actions;

namespace RecipeHelperApp.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        IFormFile ConvertToIFormFile(string filePath);
        Task<DeletionResult> DeletePhotoAsync(string publicUrl);
        Task<ImageUploadResult> DownloadImageFromUrlAsync(string imageUrl);
    }
}
