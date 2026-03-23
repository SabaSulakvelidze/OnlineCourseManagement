using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class FakePaymentGateway : IPaymentGateway
    {
        public async Task<PaymentGatewayResponse> ProcessPaymentAsync(PaymentGatewayRequest request)
        {
            var result = new PaymentGatewayResponse
            {
                TransactionId = Guid.NewGuid().ToString()
            };

            if (request.CardNumber == "4111111111111111")
            {
                result.IsSuccess = true;
                result.Status = "Success";
                result.Message = "Payment approved.";
            }
            else if (request.CardNumber == "4000000000000002")
            {
                result.IsSuccess = false;
                result.Status = "Failed";
                result.Message = "Card was declined.";
            }
            else
            {
                result.IsSuccess = false;
                result.Status = "Failed";
                result.Message = "Mock payment failed.";
            }

            return await Task.FromResult(result);
        }
    }
}
