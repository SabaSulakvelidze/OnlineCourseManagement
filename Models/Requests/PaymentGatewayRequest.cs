namespace OnlineCourseManagement.Models.Requests
{
    public class PaymentGatewayRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GEL";
        public string CardHolderName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string ExpiryMonth { get; set; } = null!;
        public string ExpiryYear { get; set; } = null!;
        public string Cvv { get; set; } = null!;
    }
}
