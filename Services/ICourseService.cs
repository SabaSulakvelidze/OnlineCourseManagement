using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface ICourseService
    {
        Task<CourseResponse> CreateCourse(CreateCourseRequest request);
        Task<List<CourseResponse>> GetAllCourses();
        Task<CourseResponse?> GetCourseById(int id);
        Task<CourseResponse?> UpdateCourse(int id, UpdateCourseRequest request);
        Task DeleteCourse(int id);
    }
}
