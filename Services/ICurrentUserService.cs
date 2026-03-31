namespace OnlineCourseManagement.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        List<string> UserPositions { get; }

    }
}
