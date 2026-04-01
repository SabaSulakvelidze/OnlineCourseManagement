namespace OnlineCourseManagement.Models.Responses
{
    public class RatingResponse
    {
        public int Id { get; set; }

        public string? Review { get; set; }

        public int Value { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual CourseResponse Course { get; set; } = null!;

        public virtual UserResponse User { get; set; } = null!;
    }
}
