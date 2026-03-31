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
        [Authorize]
        public async Task<ActionResult<LectureResponse>> CreateLecture(CreateLectureRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
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
        [Authorize]
        public async Task<ActionResult<LectureResponse>> Update(int id, UpdateLectureRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            return Ok(await lectureService.UpdateLecture(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            await lectureService.DeleteLecture(id);

            return Ok($"lecture with id {id} was deleted!");
        }

        [HttpPost("videos")]
        [Authorize]
        public async Task<ActionResult<LectureVideoResponse>> AddVideo(AddLectureVideoRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            return Ok(await lectureService.AddVideoToLecture(request));
        }

        [HttpPost("complete/{lectureId}")]
        [Authorize]
        public async Task<ActionResult<StudentsCourseResponse>> MarkLectureAsCompleted(int lectureId)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Student"))
                return Forbid();
            return Ok(await lectureService.MarkLectureAsCompleted(lectureId));
        }
    }
}
