namespace OnlineCourseManagement.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string UserPosition { get; }

    }
}
