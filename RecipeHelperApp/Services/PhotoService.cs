using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace RecipeHelperApp.Services
{
    public class PhotoService : IPhotoService 
    {
        private readonly Cloudinary _cloundinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
        }


        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            // Check if there is a file greater than 0 bytes.
            if (file.Length > 0)
            {
                // Opens a stream to read the contents of the file.
                using (var stream = file.OpenReadStream())
                {
                    // ImageUploadParams contains the parameters needed for uploading an image to Cloudinary. Here, we can specify the file and transformation.
                    var uploadParams = new ImageUploadParams
                    {
                        // Initializes a FileDescription object with the name and stream of the file. 
                        File = new FileDescription(file.FileName, stream),
                        // Specifies a tranformation.
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };

                    // Uploads an image file to Cloudinary asynchronously. 
                    uploadResult = await _cloundinary.UploadAsync(uploadParams);
                }
            }

            return uploadResult;
        }

      
        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            // Extracts the name of the file (without the extension) from a URL. '/' splits the string into an array of substrings. 
            // For instance: https://websitename.com/images/photo.jpg would become "photo" 
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            // Passes the publicId into the deletion parameters.
            var deleteParams = new DeletionParams(publicId);

            // Deletes the file from Cloudinary. 
            return await _cloundinary.DestroyAsync(deleteParams);
        }
    }
}