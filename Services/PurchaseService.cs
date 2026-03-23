using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Enums;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class PurchaseService(
        OnlineCourseManagementDbContext context,
        IPaymentGateway paymentGateway
        ) : IPurchaseService
    {
        public async Task<PurchaseResponse> BuyCourseAsync(BuyCourseRequest request)
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
                            && p.Status == (int) PurchaseStatus.Paid);

            if (alreadyPurchased)
                throw new Exception("User already purchased this course.");

            var purchase = new Purchase
            {
                UserId = request.UserId,
                CourseId = request.CourseId,
                Price = course.Price,
                Status = (int) PurchaseStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();

            var payment = new Payment
            {
                PurchaseId = purchase.Id,
                Provider = "FakeStripe",
                Amount = purchase.Price,
                Currency = course.PriceCurrency,
                Status = (int) PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            var gatewayResult = await paymentGateway.ProcessPaymentAsync(new PaymentGatewayRequest
            {
                Amount = payment.Amount,
                Currency = payment.Currency,
                CardHolderName = request.CardHolderName,
                CardNumber = request.CardNumber,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                Cvv = request.Cvv
            });

            payment.TransactionId = gatewayResult.TransactionId;

            if (gatewayResult.IsSuccess)
            {
                payment.Status = (int)PaymentStatus.Success;
                payment.PaidAt = DateTime.UtcNow;
                purchase.Status = (int)PurchaseStatus.Paid;

                // optionally enroll user after successful payment
                /*var enrollment = new StudentsCourse
                {
                    StudentId = request.UserId,
                    CourseId = request.CourseId,
                    EnrolledAt = DateTime.UtcNow,
                    Status = "Active",
                    Grade = 0,
                    Progress = 0
                };

                context.StudentsCourses.Add(enrollment);*/
            }
            else
            {
                payment.Status = (int)PaymentStatus.Failed;
                purchase.Status = (int)PurchaseStatus.Failed;
            }

            await context.SaveChangesAsync();

            return new PurchaseResponse
            {
                PurchaseId = purchase.Id,
                CourseId = purchase.CourseId,
                UserId = purchase.UserId,
                PurchaseStatus = purchase.Status.ToString(),
                PaymentStatus = payment.Status.ToString(),
                TransactionId = payment.TransactionId,
                Message = gatewayResult.Message
            };
        }
    }
}
