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

        public string UserPosition =>
             httpContextAccessor.HttpContext?.User
                    .FindFirstValue("Position")
                ?? throw new UnauthorizedAccessException("Position claim missing");
    }
}
