using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPurchaseService
    {
        Task<PurchaseResponse> BuyCourseAsync(BuyCourseRequest request);
    }
}
