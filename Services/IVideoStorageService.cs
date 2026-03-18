using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IVideoStorageService
    {
        Task<VideoUploadResponse> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<List<VideoUploadResponse>> GetAllVideos(CancellationToken cancellationToken = default);
    }
}
