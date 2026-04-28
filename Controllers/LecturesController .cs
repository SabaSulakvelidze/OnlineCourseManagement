using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController(
        ILectureService lectureService,
        ICurrentUserService currentUserService
        ) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<ActionResult<LectureResponse>> CreateLecture(CreateLectureRequest request)
        {
            return StatusCode(StatusCodes.Status201Created, await lectureService.CreateLecture(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<LectureResponse>> GetLectureById(int id)
        {
            return Ok(await lectureService.GetLectureById(id));
        }

        [HttpGet("by-course/{courseId}")]
        [Authorize]
        public async Task<ActionResult<List<LectureResponse>>> GetLectureByCourseId(int courseId)
        {
            return Ok(await lectureService.GetLectureByCourseId(courseId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<ActionResult<LectureResponse>> Update(int id, UpdateLectureRequest request)
        {
            return Ok(await lectureService.UpdateLecture(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            await lectureService.DeleteLecture(id);

            return Ok($"lecture with id {id} was deleted!");
        }

        [HttpPost("videos")]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<ActionResult<LectureVideoResponse>> AddVideo(AddLectureVideoRequest request)
        {
            return Ok(await lectureService.AddVideoToLecture(request));
        }

        [HttpPost("complete/{lectureId}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<StudentsCourseResponse>> MarkLectureAsCompleted(int lectureId)
        {
            return Ok(await lectureService.MarkLectureAsCompleted(lectureId));
        }
    }
}
