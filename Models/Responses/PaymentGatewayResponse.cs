namespace OnlineCourseManagement.Models.Responses
{
    public class PaymentGatewayResponse
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
