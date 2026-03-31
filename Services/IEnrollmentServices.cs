using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IEnrollmentServices
    {
        Task<ActionResult<StudentsCourseResponse>> EnrollStudent(StudentCourseRequest request);
        Task<ActionResult<StudentsCourseResponse>> UnenrollStudent(StudentCourseRequest request);
        Task<ActionResult<LecturersCourseResponse>> AssignLecturer(LecturerCourseRequest request);
        Task<ActionResult<LecturersCourseResponse>> UnassignLecturer(LecturerCourseRequest request);
        Task<ActionResult<StudentsCourseResponse>> TransferCourse(GiftCourseRequest request);
    }
}
