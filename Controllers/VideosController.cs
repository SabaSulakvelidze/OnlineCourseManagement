using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController(
        IVideoStorageService service,
        ICurrentUserService currentUserService) : ControllerBase
    {
        [HttpPost("upload")]
        [RequestSizeLimit(500_000_000)]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<ActionResult<VideoUploadResponse>> UploadVideo(
                                        IFormFile file,
                                        CancellationToken cancellationToken)
        {

            if (file == null)
                return BadRequest("No file was uploaded.");

            return Ok(await service.UploadVideoAsync(file, cancellationToken));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<VideoUploadResponse>> GetAllVideos(CancellationToken cancellationToken)
        {
            var result = await service.GetAllVideos(cancellationToken);

            return Ok(result);
        }
    }
}
