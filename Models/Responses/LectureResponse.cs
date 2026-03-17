namespace OnlineCourseManagement.Models.Responses
{
    public class LectureResponse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<LectureVideoResponse> Videos { get; set; } = [];
    }
}
