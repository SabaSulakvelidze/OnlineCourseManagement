namespace OnlineCourseManagement.Models.Responses
{
    public class CurrentUserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public byte[]? ProfileImage { get; set; }

        public string? ProfileImageFileName { get; set; }

        public string? ProfileImageContentType { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<LecturersCourseResponse> LecturersCourses { get; set; } = [];

        public virtual ICollection<StudentsCourseResponse> StudentsCourses { get; set; } = [];

        public virtual List<String> UsersPositions { get; set; } = [];
    }
}
