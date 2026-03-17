using System.ComponentModel.DataAnnotations;

namespace OnlineCourseManagement.Models.Requests
{
    public class UpdateCourseRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
