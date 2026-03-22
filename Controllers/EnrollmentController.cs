using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController(IEnrollmentServices service) : ControllerBase
    {
        [HttpPost("/EnrollStudent")]
        public async Task<ActionResult> EnrollStudent(EnrollStudentRequest request)
        {
            if (request == null)
                return BadRequest(request);

            return Ok(await service.EnrollStudent(request));
        }

        [HttpPost("/AssignLecturer")]
        public async Task<ActionResult> AssignLecturer(AssignLecturerRequest request)
        {
            if (request == null)
                return BadRequest(request);

            return Ok(await service.AssignLecturer(request));
        }
    }
}
