using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IEnrollmentServices
    {
        Task<ActionResult<StudentsCourseResponse>> EnrollStudent(StudentCourseRequest request);
        Task UnenrollStudent(StudentCourseRequest request);
        Task<ActionResult<LecturersCourseResponse>> AssignLecturer(LecturerCourseRequest request);
        Task UnassignLecturer(LecturerCourseRequest request);
        Task<ActionResult<StudentsCourseResponse>> TransferCourse(GiftCourseRequest request);
    }
}
