namespace OnlineCourseManagement.Models.Responses
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<LectureResponse> Lectures { get; set; } = [];
    }
}
