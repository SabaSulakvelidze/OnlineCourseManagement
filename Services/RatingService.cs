using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

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
                existing.UserId = currentUserId;
            }
            else
            {
                var rating = mapper.Map<Rating>(request);
                rating.UserId = currentUserId;
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

        public async Task<List<RatingResponse>> GetReviews(int courseId)
        {
            return await context.Ratings
                 .Where(r => r.CourseId == courseId && r.Review != null)
                .Select(r => new RatingResponse
                {
                    Review = r.Review
                })
                .ToListAsync();
        }

    }
}
