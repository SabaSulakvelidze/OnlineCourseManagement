using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Models.Responses
{
    public class LecturersCourseResponse
    {
        public DateTime AssignedAt { get; set; }

        public virtual CourseResponse Course { get; set; } = null!;

        public virtual UserResponse Lecturer { get; set; } = null!;
    }
}
