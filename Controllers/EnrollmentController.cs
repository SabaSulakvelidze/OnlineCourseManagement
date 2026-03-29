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
    public class EnrollmentController(IEnrollmentServices service) : ControllerBase
    {
        [HttpPost("/EnrollStudent")]
        
        public async Task<ActionResult<StudentsCourseResponse>> EnrollStudent(EnrollStudentRequest request)
        {
            if (request == null)
                return BadRequest(request);

            return Ok(await service.EnrollStudent(request));
        }

        [HttpPost("/AssignLecturer")]
        public async Task<ActionResult<LecturersCourseResponse>> AssignLecturer(AssignLecturerRequest request)
        {
            if (request == null)
                return BadRequest(request);

            return Ok(await service.AssignLecturer(request));
        }

        [HttpPut("transferCourse")]
        public async Task<ActionResult> TransferCourse([FromQuery] GiftCourseRequest request)
        {
            return Ok(await service.TransferCourse(request));
        }
    }
}
