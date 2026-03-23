using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request);
    }
}
