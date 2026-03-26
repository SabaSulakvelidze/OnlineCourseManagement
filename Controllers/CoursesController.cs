using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<CourseResponse>> CreateCourse(CreateCourseRequest request)
        {
            return StatusCode(StatusCodes.Status201Created, await courseService.CreateCourse(request));
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCourses()
        {
            var result = await courseService.GetAllCourses();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponse>> GetCourseById(int id)
        {
            return Ok(await courseService.GetCourseById(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponse>> UpdateCourse(int id, UpdateCourseRequest request)
        {
            return Ok(await courseService.UpdateCourse(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await courseService.DeleteCourse(id);
            
            return Ok($"Course with id {id} was deleted!");
        }
    }
}
