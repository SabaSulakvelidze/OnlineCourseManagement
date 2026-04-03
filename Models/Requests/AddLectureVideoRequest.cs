using System.ComponentModel.DataAnnotations;

namespace OnlineCourseManagement.Models.Requests
{
    public class AddLectureVideoRequest
    {
        [Required]
        public int LectureId { get; set; }

        [Required]
        [StringLength(260)]
        public string OriginalFileName { get; set; } = null!;
    }
}
