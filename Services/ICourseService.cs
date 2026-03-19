using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface ICourseService
    {
        Task<CourseResponse> CreateAsync(CreateCourseRequest request);
        Task<List<CourseResponse>> GetAllAsync();
        Task<CourseResponse?> GetByIdAsync(int id);
        Task<CourseResponse?> UpdateAsync(int id, UpdateCourseRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
