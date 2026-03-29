using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Models.Responses
{
    public class StudentsCourseResponse
    {
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public DateTime? EnrolledAt { get; set; }

        public string Status { get; set; } = null!;

        public int? Grade { get; set; }

        public int? Progress { get; set; }

        public virtual CourseResponse Course { get; set; } = null!;

        public virtual UserResponse Student { get; set; } = null!;
    }
}
