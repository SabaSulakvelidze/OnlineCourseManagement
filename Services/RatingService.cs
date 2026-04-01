using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;

namespace OnlineCourseManagement.Services
{
    public class RatingService(
        OnlineCourseManagementDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService
        ) : IRatingService
    {
        public async Task RateCourse(RateCourseRequest request)
        {
            var currentUserId = currentUserService.UserId;

            var existing = await context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == currentUserId
                                      && r.CourseId == request.CourseId);

            if (existing != null)
            {
                mapper.Map(request, existing);
            }
            else
            {
                var rating = mapper.Map<Rating>(request);
                await context.Ratings.AddAsync(rating);
            }

            await context.SaveChangesAsync();
        }

        public async Task<double> GetAverage(int courseId)
        {
            return await context.Ratings
                .Where(r => r.CourseId == courseId)
                .AverageAsync(r => (double?)r.Value) ?? 0;
        }

    }
}
