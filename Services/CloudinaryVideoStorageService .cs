using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using OnlineCourseManagement.Models.CloudeStorage;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class CloudinaryVideoStorageService : IVideoStorageService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryVideoStorageService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            cloudinary = new Cloudinary(account);
        }

        public async Task<VideoUploadResponse> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv", ".webm" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Unsupported video format.");

            await using var stream = file.OpenReadStream();

            var publicId = Guid.NewGuid().ToString();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "course-videos",
                PublicId = publicId,
                Overwrite = false
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary upload failed: {uploadResult.Error.Message}");

            return new VideoUploadResponse(
                uploadResult.SecureUrl?.ToString()
                    ?? throw new Exception("Cloudinary did not return a secure URL."),
                uploadResult.PublicId
            );
        }

        public async Task<List<VideoUploadResponse>> GetAllVideos(CancellationToken cancellationToken = default)
        {
            var allVideos = new List<VideoUploadResponse>();
            string? nextCursor = null;

            do
            {
                var parameters = new ListResourcesByPrefixParams
                {
                    ResourceType = ResourceType.Video,
                    Type = "upload",
                    Prefix = "course-videos/",
                    MaxResults = 100,
                    NextCursor = nextCursor
                };

                var result = await cloudinary.ListResourcesAsync(parameters);

                if (result.Error != null)
                    throw new Exception($"Cloudinary fetch failed: {result.Error.Message}");

                allVideos.AddRange(
                    result.Resources.Select(video => new VideoUploadResponse(
                        video.SecureUrl?.ToString() ?? string.Empty,
                        video.PublicId
                    ))
                );

                nextCursor = result.NextCursor;

            } while (!string.IsNullOrWhiteSpace(nextCursor));

            return allVideos;
        }
    }
}
