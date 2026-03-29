using OnlineCourseManagement.Models.Requests;

namespace OnlineCourseManagement.Services
{
    public interface IRatingService
    {
        public Task RateCourse(RateCourseRequest request);
        Task<double> GetAverage(int courseId);
    }
}
