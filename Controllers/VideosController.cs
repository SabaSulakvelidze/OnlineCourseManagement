using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController(IVideoStorageService service) : ControllerBase
    {
        [HttpPost("upload")]
        [RequestSizeLimit(500_000_000)]
        public async Task<ActionResult<VideoUploadResponse>> UploadVideo(
                                        IFormFile file,
                                        CancellationToken cancellationToken)
        {
            if (file == null)
                return BadRequest("No file was uploaded.");

            var result = await service.UploadVideoAsync(file, cancellationToken);

            return Ok(result);
        }
    }
}
