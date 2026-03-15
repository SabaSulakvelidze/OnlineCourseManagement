using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController(IVideoStorageService service) : ControllerBase
    {
        [HttpPost("upload")]
        [RequestSizeLimit(500_000_000)] // 500 MB example
        public async Task<IActionResult> UploadVideo(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
                return BadRequest("No file was uploaded.");

            var url = await service.UploadVideoAsync(file, cancellationToken);

            return Ok(new
            {
                Message = "Video uploaded successfully.",
                VideoUrl = url
            });
        }
    }
}
