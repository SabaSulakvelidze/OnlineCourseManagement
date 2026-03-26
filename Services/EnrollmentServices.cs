using AutoMapper;
using FinalProject.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Enums;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class EnrollmentServices(
        OnlineCourseManagementDbContext context,
        IMapper mapper
        ) : IEnrollmentServices
    {
        public async Task<ActionResult<LecturersCourseResponse>> AssignLecturer(AssignLecturerRequest request)
        {
            if (!await context.Courses.AnyAsync(c => c.Id == request.CourseId))
                throw new ElementNotFoundException($"Course with id {request.CourseId} was not found.");

            var lecturer = await context.Users
                .Include(u => u.UsersPositions)
                .FirstOrDefaultAsync(u => u.Id == request.LecturerId)
                ?? throw new ElementNotFoundException($"User with id {request.LecturerId} was not found.");

            if (lecturer.UsersPositions.Select(up => up.Position.PositionName).ToList().Contains("Lecturer"))
                throw new ConflictException($"User with id {request.LecturerId} is not a lecturer.");

            var alreadyAssigned = await context.LecturersCourses
                .AnyAsync(lc => lc.CourseId == request.CourseId && lc.LecturerId == request.LecturerId);

            if (alreadyAssigned)
                throw new ConflictException("This lecturer is already assigned to the course.");

            var entity = mapper.Map<LecturersCourse>(request);

            entity.AssignedAt = DateTime.UtcNow;

            context.LecturersCourses.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<LecturersCourseResponse>(entity);
        }

        public async Task<ActionResult<StudentsCourseResponse>> EnrollStudent(EnrollStudentRequest request)
        {
            var courseExists = await context.Courses
                 .AnyAsync(c => c.Id == request.CourseId);

            if (!courseExists)
                throw new ElementNotFoundException($"Course with id {request.CourseId} was not found.");

            var student = await context.Users
                .FirstOrDefaultAsync(u => u.Id == request.StudentId)
                ?? throw new ElementNotFoundException($"User with id {request.StudentId} was not found.");

            if (student.UsersPositions.Select(up => up.Position.PositionName).ToList().Contains("Student"))
                throw new ConflictException($"User with id {request.StudentId} is not a student.");

            var alreadyEnrolled = await context.StudentsCourses
               .AnyAsync(sc => sc.CourseId == request.CourseId && sc.StudentId == request.StudentId);

            if (alreadyEnrolled)
                throw new ConflictException("This student is already enrolled in the course.");

            if(!await context.Purchases.AnyAsync(p => p.UserId == request.StudentId && p.CourseId == request.CourseId))
                throw new ElementNotFoundException($"Student with id {request.StudentId}, has not paid for course with id {request.CourseId}");

            var entity = mapper.Map<StudentsCourse>(request);

            entity.EnrolledAt = DateTime.UtcNow;
            entity.Status = (int)StudentStatus.Active;

            context.StudentsCourses.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<StudentsCourseResponse>(entity);
        }
    }
}
