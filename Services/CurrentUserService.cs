using System.Security.Claims;

namespace OnlineCourseManagement.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {

        public int UserId =>
            int.Parse(
                httpContextAccessor.HttpContext?.User
                    .FindFirstValue("UserId")
                ?? throw new UnauthorizedAccessException("UserId claim missing")
            );

        public List<string> UserPositions =>
             httpContextAccessor.HttpContext?.User
                .Claims
                .Where(item => item.Type == "Position")
                .Select(item => item.Value)
                .ToList()
                ?? throw new UnauthorizedAccessException("Position claim missing");
    }
}
