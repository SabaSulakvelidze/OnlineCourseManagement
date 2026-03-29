using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(ICourseService service,IRatingService ratingService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CourseResponse>> CreateCourse(CreateCourseRequest request)
        {
            return StatusCode(StatusCodes.Status201Created, await service.CreateCourse(request));
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCourses()
        {
            var result = await service.GetAllCourses();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponse>> GetCourseById(int id)
        {
            return Ok(await service.GetCourseById(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponse>> UpdateCourse(int id, UpdateCourseRequest request)
        {
            return Ok(await service.UpdateCourse(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await service.DeleteCourse(id);
            
            return Ok($"Course with id {id} was deleted!");
        }

        [HttpPost("buyCourse")]
        public async Task<ActionResult<PurchaseCourseResponse>> BuyCourse(PurchaseCourseRequest request)
        {
            var result = await service.BuyCourseAsync(request);
            return Ok(result);
        }

        [HttpGet("getUsersCourses")]
        public async Task<ActionResult<UsersCourses>> GetUsersCourses([FromQuery] int userId)
        {
            return Ok(await service.GetUsersCourses(userId));
        }

        [HttpPost("Review")]

        public async Task<ActionResult> CourseReview(RateCourseRequest request)
        {
            var permision = User.Claims.Where(item => item.Type == "Position").Select(item => item.Value).ToList();

            if (!permision.Contains("Student"))
            {
                return Unauthorized("You have not permision to change");
            }

            await ratingService.RateCourse(request);
            return Ok("Rated successfully");
        }

        [HttpGet("average/{courseId}")]
        public async Task<IActionResult> GetAverage(int courseId)
        {


            var avg = await ratingService.GetAverage(courseId);
            return Ok(avg);
        }

        [HttpGet("reviews/{courseId}")]
        public async Task<IActionResult> GetReviews(int courseId)
        {
            var result = await ratingService.GetReviews(courseId);
            return Ok(result);
        }

    }
}
