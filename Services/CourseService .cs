using AutoMapper;
using CloudinaryDotNet;
using OnlineCourseManagement.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Enums;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class CourseService(
        OnlineCourseManagementDbContext context,
        IPaymentGateway paymentGateway,
        IMapper mapper,
        ICurrentUserService currentUserService) :ICourseService
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
                ?? throw new ElementNotFoundException($"Course with id {id} was not found");

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

        public async Task<PurchaseCourseResponse> BuyCourseAsync(PurchaseCourseRequest request)
        {
            var currentUserId = currentUserService.UserId;

            var course = await context.Courses
                .FirstOrDefaultAsync(c => c.Id == request.CourseId)
                ?? throw new ElementNotFoundException($"Course with id {request.CourseId} was not found.");

            var alreadyPurchased = await context.Purchases
                .AnyAsync(p => p.UserId == currentUserId
                            && p.CourseId == request.CourseId
                            && p.Status == (int)PurchaseStatus.Paid);

            if (alreadyPurchased)
                throw new ConflictException("User already purchased this course.");

            var purchase = new Purchase
            {
                UserId = currentUserId,
                CourseId = request.CourseId,
                Price = course.Price,
                Currency = course.PriceCurrency,
                Status = (int)PurchaseStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();

            var gatewayResult = await paymentGateway.ProcessPaymentAsync(new PaymentGatewayRequest
            {
                Amount = course.Price,
                Currency = course.PriceCurrency,
                CardHolderName = request.CardHolderName,
                CardNumber = request.CardNumber,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Cvv = request.Cvv
            });

            if (gatewayResult.IsSuccess) purchase.Status = (int)PurchaseStatus.Paid;
            else purchase.Status = (int)PurchaseStatus.Failed;

            await context.SaveChangesAsync();

            return mapper.Map<PurchaseCourseResponse>(purchase);
        }

        public async Task<List<UsersCourses>> GetUsersCourses(int userId)
        {
            var sqlParams = new SqlParameter("@UserId", userId);
            var result = await context.Set<UsersCourses>().FromSqlRaw("EXEC GetUsersCourses @UserId", sqlParams).ToListAsync();

            return result;
        }

    }
}
