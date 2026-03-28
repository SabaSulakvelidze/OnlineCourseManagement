using AutoMapper;
using FinalProject.Exceptions;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class LectureService(
        OnlineCourseManagementDbContext context,
        IMapper mapper
        ) : ILectureService
    {
        public async Task<LectureResponse?> CreateLecture(CreateLectureRequest request)
        {
            if (!await context.Courses.AnyAsync(c => c.Id == request.CourseId))
                throw new ElementNotFoundException($"Course with id {request.CourseId} was not found.");
            var result = mapper.Map<Lecture>(request);
            result.CreatedAt = DateTime.UtcNow;

            context.Lectures.Add(result);
            await context.SaveChangesAsync();

            return mapper.Map<LectureResponse>(result);
        }

        public async Task<List<LectureResponse>> GetLectureByCourseId(int courseId)
        {
            var lectures = await context.Lectures
                .Where(l => l.CourseId == courseId)
                .Include(l => l.LectureVideos)
                .ToListAsync();

            return mapper.Map<List<LectureResponse>>(lectures);
        }

        public async Task<LectureResponse?> GetLectureById(int id)
        {
            var lecture = await context.Lectures
                .Include(l => l.LectureVideos)
                .FirstOrDefaultAsync(l => l.Id == id)
                ?? throw new ElementNotFoundException($"Lecture with id {id} was not found");

            return mapper.Map<LectureResponse>(lecture);
        }

        public async Task<LectureResponse?> UpdateLecture(int id, UpdateLectureRequest request)
        {
            var lecture = await context.Lectures
               .Include(l => l.LectureVideos)
               .FirstOrDefaultAsync(l => l.Id == id)
               ?? throw new ElementNotFoundException($"Lecture with id {id} was not found");

            mapper.Map(request, lecture);

            await context.SaveChangesAsync();

            return mapper.Map<LectureResponse>(lecture);
        }

        public async Task DeleteLecture(int id)
        {
            var lecture = await context.Lectures
                .Include(l => l.LectureVideos)
                .FirstOrDefaultAsync(l => l.Id == id)
                ?? throw new ElementNotFoundException($"Lecture with id {id} was not found");

            context.Lectures.Remove(lecture);
            await context.SaveChangesAsync();
        }

        public async Task<LectureVideoResponse?> AddVideoToLecture(AddLectureVideoRequest request)
        {
            if (!await context.Lectures.AnyAsync(c => c.Id == request.LectureId))
                throw new ElementNotFoundException($"Lecture with id {request.LectureId} was not found.");

            var result = mapper.Map<LectureVideo>(request);
            result.UploadedAt = DateTime.UtcNow;

            context.LectureVideos.Add(result);
            await context.SaveChangesAsync();

            return mapper.Map<LectureVideoResponse>(result);
        }

    }
}
