using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class LectureService(
        OnlineCourseManagementDbContext context,
        IMapper mapper
        ) : ILectureService
    {
        public async Task<LectureResponse?> CreateAsync(CreateLectureRequest request)
        {
            var courseExists = await context.Courses.AnyAsync(c => c.Id == request.CourseId);
            if (!courseExists)
                return null;

            var lecture = new Lecture
            {
                CourseId = request.CourseId,
                Title = request.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            context.Lectures.Add(lecture);
            await context.SaveChangesAsync();

            return await GetByIdAsync(lecture.Id);
        }

        public async Task<List<LectureResponse>> GetByCourseIdAsync(int courseId)
        {
            var lectures = await context.Lectures
                .AsNoTracking()
                .Where(l => l.CourseId == courseId)
                .Include(l => l.LectureVideos)
                .ToListAsync();

            return mapper.Map<List<LectureResponse>>(lectures);
        }

        public async Task<LectureResponse?> GetByIdAsync(int id)
        {
            var lecture = await context.Lectures
                .AsNoTracking()
                .Include(l => l.LectureVideos)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lecture == null ? null : mapper.Map<LectureResponse>(lecture);
        }

        public async Task<LectureResponse?> UpdateAsync(int id, UpdateLectureRequest request)
        {
            var lecture = await context.Lectures.FirstOrDefaultAsync(l => l.Id == id);
            if (lecture == null)
                return null;

            lecture.Title = request.Title.Trim();
            lecture.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

            await context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lecture = await context.Lectures.FirstOrDefaultAsync(l => l.Id == id);
            if (lecture == null)
                return false;

            context.Lectures.Remove(lecture);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<LectureVideoResponse?> AddVideoAsync(AddLectureVideoRequest request)
        {
            var lectureExists = await context.Lectures.AnyAsync(l => l.Id == request.LectureId);
            if (!lectureExists)
                return null;

            var lectureVideo = new LectureVideo
            {
                LectureId = request.LectureId,
                OriginalFileName = request.OriginalFileName.Trim(),
                VideoUrl = request.VideoUrl.Trim(),
                PublicId = request.PublicId.Trim(),
                UploadedAt = DateTime.UtcNow
            };

            context.LectureVideos.Add(lectureVideo);
            await context.SaveChangesAsync();

            return new LectureVideoResponse
            {
                Id = lectureVideo.Id,
                OriginalFileName = lectureVideo.OriginalFileName,
                VideoUrl = lectureVideo.VideoUrl,
                PublicId = lectureVideo.PublicId,
                UploadedAtUtc = lectureVideo.UploadedAt
            };
        }

    }
}
