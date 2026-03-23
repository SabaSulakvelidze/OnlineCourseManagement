namespace OnlineCourseManagement.Models.Responses
{
    public class PurchaseResponse
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string PurchaseStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string? TransactionId { get; set; }
        public string Message { get; set; } = null!;
    }
}
