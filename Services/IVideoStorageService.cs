namespace OnlineCourseManagement.Services
{
    public interface IVideoStorageService
    {
        Task<string> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}
