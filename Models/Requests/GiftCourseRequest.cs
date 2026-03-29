namespace OnlineCourseManagement.Models.Requests
{
    public class GiftCourseRequest
    {
        public required int RecipientId { get; set; }
        public required int CourseId { get; set; }
    }
}
