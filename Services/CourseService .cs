using AutoMapper;
using CloudinaryDotNet;
using FinalProject.Exceptions;
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
        public async Task<CourseResponse> CreateCourse(CreateCourseRequest request)
        {
            var result = mapper.Map<Course>(request);
            result.CreatedAt = DateTime.UtcNow;

            context.Courses.Add(result);
            await context.SaveChangesAsync();

            return mapper.Map<CourseResponse>(result);
        }

        public async Task<List<CourseResponse>> GetAllCourses()
        {
            var courses = await context.Courses
                .Include(c => c.Lectures)
                    .ThenInclude(l => l.LectureVideos)
                .ToListAsync();

            return mapper.Map<List<CourseResponse>>(courses);
        }

        public async Task<CourseResponse?> GetCourseById(int id)
        {
            var course = await context.Courses
                .Include(c => c.Lectures)
                    .ThenInclude(l => l.LectureVideos)
                .FirstOrDefaultAsync(c => c.Id == id) 
                ?? throw new ElementNotFoundException($"Course with id {id} was not found"); ;

            return mapper.Map<CourseResponse>(course);
        }

        public async Task<CourseResponse?> UpdateCourse(int id, UpdateCourseRequest request)
        {
            var course = await context.Courses.FindAsync(id)
                ?? throw new ElementNotFoundException($"Course with id {id} was not found");

            if (await context.Courses.AnyAsync(p => p.Title == request.Title))
                throw new ConflictException($"Course with title '{request.Title}' already exists");


            mapper.Map(request, course);

            await context.SaveChangesAsync();

            return mapper.Map<CourseResponse>(course);
        }

        public async Task DeleteCourse(int id)
        {
            var course = await context.Courses.FindAsync(id)
                 ?? throw new ElementNotFoundException($"Course with id {id} was not found");

            var lectureId = await context.Lectures
                .Where(l => l.CourseId == id)
                .Select(l => l.Id)
                .ToListAsync();

            if (lectureId.Count != 0)
                throw new ConflictException($"Course with id {id} has lectures: {string.Join(", ", lectureId)}");

            context.Courses.Remove(course);
            await context.SaveChangesAsync();

        }

    }
}
