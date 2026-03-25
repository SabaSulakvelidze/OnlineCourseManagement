namespace OnlineCourseManagement.Models.Responses
{
    public class PurchaseCourseResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
