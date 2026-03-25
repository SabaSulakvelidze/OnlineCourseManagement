namespace OnlineCourseManagement.Models.Requests
{
    public class PurchaseCourseRequest
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public string CardHolderName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string ExpiryMonth { get; set; } = null!;
        public string ExpiryYear { get; set; } = null!;
        public string Cvv { get; set; } = null!;
    }
}
