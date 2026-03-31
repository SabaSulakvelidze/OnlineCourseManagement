using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;
using System.Security;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(
        ICourseService service,
        ICurrentUserService currentUserService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CourseResponse>> CreateCourse(CreateCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            return StatusCode(StatusCodes.Status201Created, await service.CreateCourse(request));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCourses()
        {
            var result = await service.GetAllCourses();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CourseResponse>> GetCourseById(int id)
        {
            return Ok(await service.GetCourseById(id));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CourseResponse>> UpdateCourse(int id, UpdateCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            return Ok(await service.UpdateCourse(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Lecturer"))
                return Forbid();
            await service.DeleteCourse(id);
            
            return Ok($"Course with id {id} was deleted!");
        }

        [HttpPost("buyCourse")]
        [Authorize]
        public async Task<ActionResult<PurchaseCourseResponse>> BuyCourse(PurchaseCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Student"))
                return Forbid();
            var result = await service.BuyCourseAsync(request);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("getUsersCourses")]
        [Authorize]
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
            var permision = User.Claims.Where(item => item.Type == "Position").Select(item => item.Value).ToList();

            if (!permision.Contains("Student"))
            {
                return Unauthorized("You have not permision to change");
            }

            var avg = await ratingService.GetAverage(courseId);
            return Ok(avg);
        }


    }
}
