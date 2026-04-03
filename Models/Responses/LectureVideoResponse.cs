namespace OnlineCourseManagement.Models.Responses
{
    public class LectureVideoResponse
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
    }
}
