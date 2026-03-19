using System.ComponentModel.DataAnnotations;

namespace OnlineCourseManagement.Models.Requests
{
    public class CreateLectureRequest
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
