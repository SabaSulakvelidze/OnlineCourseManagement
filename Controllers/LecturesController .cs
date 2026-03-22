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
    public class LecturesController(ILectureService lectureService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<LectureResponse>> CreateLecture(CreateLectureRequest request)
        {
            return StatusCode(StatusCodes.Status201Created, await lectureService.CreateLecture(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LectureResponse>> GetLectureById(int id)
        {
            return Ok(await lectureService.GetLectureById(id));
        }

        [HttpGet("by-course/{courseId}")]
        public async Task<ActionResult<List<LectureResponse>>> GetLectureByCourseId(int courseId)
        {
            return Ok(await lectureService.GetLectureByCourseId(courseId));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LectureResponse>> Update(int id, UpdateLectureRequest request)
        {
            return Ok(await lectureService.UpdateLecture(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            await lectureService.DeleteLecture(id);

            return Ok($"lecture with id {id} was deleted!");
        }

        [HttpPost("videos")]
        public async Task<ActionResult<LectureVideoResponse>> AddVideo(AddLectureVideoRequest request)
        {
            return Ok(await lectureService.AddVideoToLecture(request));
        }
    }
}
