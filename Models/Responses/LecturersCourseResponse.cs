using FinalProject.Models.Responses;

namespace OnlineCourseManagement.Models.Responses
{
    public class LecturersCourseResponse
    {
        public int LecturerId { get; set; }

        public int CourseId { get; set; }

        public DateTime AssignedAt { get; set; }

        public virtual CourseResponse Course { get; set; } = null!;

        public virtual UserResponse Lecturer { get; set; } = null!;
    }
}
