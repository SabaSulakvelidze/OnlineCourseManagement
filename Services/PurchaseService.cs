using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Enums;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class PurchaseService(
        OnlineCourseManagementDbContext context,
        IPaymentGateway paymentGateway,
        IMapper mapper
        ) : IPurchaseService
    {
        public async Task<PurchaseCourseResponse> BuyCourseAsync(PurchaseCourseRequest request)
        {
            var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId)
            ?? throw new Exception($"User with id {request.UserId} was not found.");

            var course = await context.Courses
                .FirstOrDefaultAsync(c => c.Id == request.CourseId)
                ?? throw new Exception($"Course with id {request.CourseId} was not found.");

            var alreadyPurchased = await context.Purchases
                .AnyAsync(p => p.UserId == request.UserId
                            && p.CourseId == request.CourseId
                            && p.Status == (int)PurchaseStatus.Paid);

            if (alreadyPurchased)
                throw new Exception("User already purchased this course.");

            var purchase = new Purchase
            {
                UserId = request.UserId,
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
        
    }
}
