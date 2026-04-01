namespace OnlineCourseManagement.Models.Responses
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; } = null!;
        public decimal? Rating { get; set; }
    }
}
