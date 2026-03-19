using AutoMapper;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class CourseService(
        OnlineCourseManagementDbContext context,
        IMapper mapper) :ICourseService
    {
        public async Task<CourseResponse> CreateAsync(CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            context.Courses.Add(course);
            await context.SaveChangesAsync();

            return mapper.Map<CourseResponse>(course);
        }

        public async Task<List<CourseResponse>> GetAllAsync()
        {
            var courses = await context.Courses
                .AsNoTracking()
                .Include(c => c.Lectures)
                    .ThenInclude(l => l.LectureVideos)
                .OrderBy(c => c.Id)
                .ToListAsync();

            return mapper.Map<List<CourseResponse>>(courses);
        }

        public async Task<CourseResponse?> GetByIdAsync(int id)
        {
            var course = await context.Courses
                .AsNoTracking()
                .Include(c => c.Lectures)
                    .ThenInclude(l => l.LectureVideos)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course == null ? null : mapper.Map<CourseResponse>(course);
        }

        public async Task<CourseResponse?> UpdateAsync(int id, UpdateCourseRequest request)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                return null;

            course.Title = request.Title.Trim();
            course.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

            await context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                return false;

            context.Courses.Remove(course);
            await context.SaveChangesAsync();

            return true;
        }

    }
}
