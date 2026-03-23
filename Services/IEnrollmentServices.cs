using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IEnrollmentServices
    {
        Task<ActionResult<StudentsCourseResponse>> EnrollStudent(EnrollStudentRequest request);

        Task<ActionResult<LecturersCourseResponse>> AssignLecturer(AssignLecturerRequest request);
    }
}
