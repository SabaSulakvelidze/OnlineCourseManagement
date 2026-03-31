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
        IEnrollmentServices service,
        ICurrentUserService currentUserService
        ) : ControllerBase
    {
        [HttpPost("/EnrollStudent")]
        [Authorize]
        public async Task<ActionResult<StudentsCourseResponse>> EnrollStudent(EnrollStudentRequest request)
        {
            return Ok(await service.EnrollStudent(request));
        }

        [HttpPost("/AssignLecturer")]
        [Authorize]
        public async Task<ActionResult<LecturersCourseResponse>> AssignLecturer(AssignLecturerRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await service.AssignLecturer(request));
        }

        [HttpPut("transferCourse")]
        [Authorize]
        public async Task<ActionResult> TransferCourse([FromQuery] GiftCourseRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin") && !positions.Contains("Student"))
                return Forbid();
            return Ok(await service.TransferCourse(request));
        }
    }
}
