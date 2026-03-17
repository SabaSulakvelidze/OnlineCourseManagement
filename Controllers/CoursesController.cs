using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(ICourseService courseService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CourseResponse>> Create([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await courseService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseResponse>>> GetAll()
        {
            var result = await courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseResponse>> GetById(int id)
        {
            var result = await courseService.GetByIdAsync(id);
            if (result == null)
                return NotFound($"Course with id {id} was not found.");

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CourseResponse>> Update(int id, [FromBody] UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await courseService.UpdateAsync(id, request);
            if (result == null)
                return NotFound($"Course with id {id} was not found.");

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await courseService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Course with id {id} was not found.");

            return NoContent();
        }
    }
}
