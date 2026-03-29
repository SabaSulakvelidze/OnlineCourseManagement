using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IRatingService
    {
        public Task RateCourse(RateCourseRequest request);
        Task<double> GetAverage(int courseId);
        Task<List<RatingResponse>> GetReviews(int courseId);

    }
}
