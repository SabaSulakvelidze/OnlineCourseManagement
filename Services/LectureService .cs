using AutoMapper;
using Azure.Core;
using FinalProject.Exceptions;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class LectureService(
        OnlineCourseManagementDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService
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

        public async Task<StudentsCourseResponse> MarkLectureAsCompleted(int lectureId)
        {
            var userId = currentUserService.UserId;

            var lecture = await context.Lectures
               .FirstOrDefaultAsync(l => l.Id == lectureId)
               ?? throw new ElementNotFoundException($"Lecture with id {lectureId} was not found");

            var studentCourse = await context.StudentsCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == userId && sc.CourseId == lecture.CourseId)
                ?? throw new ConflictException($"incorrect Lecture id: {lectureId}");

            var studentLectureProgress = await context.StudentLectureProgresses
                .FirstOrDefaultAsync(slp => slp.StudentId == userId && slp.LectureId == lectureId);

            if (studentLectureProgress != null)
                return mapper.Map<StudentsCourseResponse>(studentCourse);

            studentLectureProgress = new()
            {
                StudentId = userId,
                LectureId = lectureId,
                CompletedAt = DateTime.UtcNow
            };
            context.StudentLectureProgresses.Add(studentLectureProgress);

            await context.SaveChangesAsync();

            var totalLectures = await context.Lectures
                .CountAsync(l => l.CourseId == lecture.CourseId);

            var completedLectures = await context.StudentLectureProgresses
                .CountAsync(x =>x.StudentId == userId && x.Lecture.CourseId == lecture.CourseId);

            studentCourse.Progress = totalLectures != 0 ? (int)(completedLectures * 100 / totalLectures) : 0;

            await context.SaveChangesAsync();
            return mapper.Map<StudentsCourseResponse>(studentCourse);

        }
    }
}
