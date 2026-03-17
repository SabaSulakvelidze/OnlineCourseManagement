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
        public async Task<ActionResult<LectureResponse>> Create([FromBody] CreateLectureRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await lectureService.CreateAsync(request);
                if (result == null)
                    return NotFound($"Course with id {request.CourseId} was not found.");

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LectureResponse>> GetById(int id)
        {
            var result = await lectureService.GetByIdAsync(id);
            if (result == null)
                return NotFound($"Lecture with id {id} was not found.");

            return Ok(result);
        }

        [HttpGet("by-course/{courseId:int}")]
        public async Task<ActionResult<List<LectureResponse>>> GetByCourseId(int courseId)
        {
            var result = await lectureService.GetByCourseIdAsync(courseId);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<LectureResponse>> Update(int id, [FromBody] UpdateLectureRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await lectureService.UpdateAsync(id, request);
                if (result == null)
                    return NotFound($"Lecture with id {id} was not found.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await lectureService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Lecture with id {id} was not found.");

            return NoContent();
        }

        [HttpPost("videos")]
        public async Task<ActionResult<LectureVideoResponse>> AddVideo([FromBody] AddLectureVideoRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await lectureService.AddVideoAsync(request);
            if (result == null)
                return NotFound($"Lecture with id {request.LectureId} was not found.");

            return Ok(result);
        }
    }
}
