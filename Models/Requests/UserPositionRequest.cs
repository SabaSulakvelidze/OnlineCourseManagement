namespace OnlineCourseManagement.Models.Requests
{
    public class UserPositionRequest
    {
        public required int UserId { get; set; }
        public required Guid PositionId { get; set; }
    }
}
