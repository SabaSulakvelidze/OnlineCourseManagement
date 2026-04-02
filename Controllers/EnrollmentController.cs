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
    public class EnrollmentController(
        IEnrollmentServices enrollmentServices,
        ICurrentUserService currentUserService
        ) : ControllerBase
    {
        [HttpPost("EnrollStudent")]
        [Authorize]
        public async Task<ActionResult<StudentsCourseResponse>> EnrollStudent([FromQuery] StudentCourseRequest request)
        {
            return Ok(await enrollmentServices.EnrollStudent(request));
        }

        [HttpPost("AssignLecturer")]
        [Authorize]
        public async Task<ActionResult<LecturersCourseResponse>> AssignLecturer([FromQuery] LecturerCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await enrollmentServices.AssignLecturer(request));
        }

        [HttpDelete("unenrollStudent")]
        public async Task<ActionResult<StudentsCourseResponse>> UnenrollStudent([FromQuery] StudentCourseRequest request)
        {
            await enrollmentServices.UnenrollStudent(request);
            return Ok();
        }

        [HttpDelete("unassignLecturer")]
        public async Task<ActionResult<LecturersCourseResponse>> UnassignLecturer([FromQuery] LecturerCourseRequest request)
        {
            await enrollmentServices.UnassignLecturer(request);
            return Ok();
        }

        [HttpPut("giftCourse")]
        [Authorize]
        public async Task<ActionResult> giftCourse([FromQuery] GiftCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Student"))
                return Forbid();
            return Ok(await enrollmentServices.TransferCourse(request));
        }
    }
}
