using AutoMapper;
using OnlineCourseManagement.Exceptions;
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
        IMapper mapper,
        ICurrentUserService currentUserService
        ) : IEnrollmentServices
    {
        public async Task<ActionResult<LecturersCourseResponse>> AssignLecturer(AssignLecturerRequest request)
        {
            if (!await context.Courses.AnyAsync(c => c.Id == request.CourseId))
                throw new ElementNotFoundException($"Course with id {request.CourseId} was not found.");

            var lecturer = await context.Users
                .Include(u => u.UsersPositions)
                    .ThenInclude(up => up.Position)
                .FirstOrDefaultAsync(u => u.Id == request.LecturerId)
                ?? throw new ElementNotFoundException($"User with id {request.LecturerId} was not found.");

            if (!lecturer.UsersPositions.Any(up => up.Position.PositionName == "Lecturer"))
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

            if (!student.UsersPositions.Any(up => up.Position.PositionName == "Student"))
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

        public async Task<ActionResult<StudentsCourseResponse>> TransferCourse(GiftCourseRequest request)
        {
            var currentUser = currentUserService.UserId;

            if (!await context.Courses.AnyAsync(c => c.Id == request.CourseId))
                throw new ElementNotFoundException($"Course with id {request.CourseId} was not found");

            var studentCourse = await context.StudentsCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == currentUser && sc.CourseId == request.CourseId) 
                ?? throw new ConflictException($"You have not purchased Course with id {request.CourseId} yet!");

            if (currentUser == request.RecipientId)
                throw new ConflictException("You cannot transfer a course to yourself.");

            var recipientUser = await context.Users
                .AnyAsync(u =>
                    u.Id == request.RecipientId &&
                    u.UsersPositions.Any(up => up.Position.PositionName == "Student"));
            if(recipientUser) 
                throw new ConflictException($"Recipient with id {request.RecipientId} was not found or is not a student.");

            if (await context.StudentsCourses.AnyAsync(sc => sc.StudentId == request.RecipientId && sc.CourseId == request.CourseId))
                throw new ConflictException($"Recipient already owns course with id {request.CourseId}");

            studentCourse.StudentId = request.RecipientId;

            await context.SaveChangesAsync();

            return mapper.Map<StudentsCourseResponse>(studentCourse);
        }
    }
}
