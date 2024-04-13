using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

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


        // Take a URL and download the image as a IFile.
        // It was easy to overthink this approach. DALL-E generates a unique URL,
        // temporarily available for one hour. To  save the images DALL-E generates 
        // to another service  such as CLoudinary, it was neccessary to downloa dthe
        // image from the URL. Fortunately ASP.NET Core has a special method called
        // "DownloadFileTaskAsync" which can be utilized to download the image to a temporary
        // file. 
        public async Task<ImageUploadResult> DownloadImageFromUrlAsync(string imageUrl)
        {
            // Download the image
            using (var client = new WebClient())
            {
                // Generate a unique file name
                string fileName = Path.GetTempFileName();

                // Download the image to the temporary file
                await client.DownloadFileTaskAsync(imageUrl, fileName);

                // Convert the downloaded file to IFormFile.
                // This is a custom method. 
                IFormFile file = ConvertToIFormFile(fileName);

                // Call the method to add the photo
                return await AddPhotoAsync(file);
            }
        }

        public IFormFile ConvertToIFormFile(string filePath)
        {
            // Open the file stream
            var stream = new FileStream(filePath, FileMode.Open);

            // Create an instance of FormFile
            // Update content type accordingly. DALL-E's images are saved as .png by default.
            // Therefore, we specify the content type as image/png. 
            var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png" 
            };

            return formFile;
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