using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface ILectureService
    {
        Task<LectureResponse?> CreateAsync(CreateLectureRequest request);
        Task<List<LectureResponse>> GetByCourseIdAsync(int courseId);
        Task<LectureResponse?> GetByIdAsync(int id);
        Task<LectureResponse?> UpdateAsync(int id, UpdateLectureRequest request);
        Task<bool> DeleteAsync(int id);
        Task<LectureVideoResponse?> AddVideoAsync(AddLectureVideoRequest request);
    }
}
