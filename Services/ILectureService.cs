using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface ILectureService
    {
        Task<LectureResponse?> CreateLecture(CreateLectureRequest request);
        Task<List<LectureResponse>> GetLectureByCourseId(int courseId);
        Task<LectureResponse?> GetLectureById(int id);
        Task<LectureResponse?> UpdateLecture(int id, UpdateLectureRequest request);
        Task DeleteLecture(int id);
        Task<LectureVideoResponse?> AddVideoToLecture(AddLectureVideoRequest request);
    }
}
