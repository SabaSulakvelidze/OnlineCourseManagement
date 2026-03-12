namespace OnlineCourseManagement.Models.Responses
{
    public class UserProfileImageResponse
    {
        public byte[] ImageBytes { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string? FileName { get; set; }
    }
}
