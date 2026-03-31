namespace OnlineCourseManagement.Models.Requests
{
    public class ChangePositionRequest
    {
        public int UsersId { get; set; }
        public Guid PositionId { get; set; }
    }
}
