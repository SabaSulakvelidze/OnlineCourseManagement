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

        [Required]
        [StringLength(1000)]
        public string VideoUrl { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string PublicId { get; set; } = null!;
    }
}
