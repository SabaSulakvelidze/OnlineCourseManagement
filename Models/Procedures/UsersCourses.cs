namespace OnlineCourseManagement.Models.Procedures
{
    public class UsersCourses
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int? LecturerOf { get; set; }
        public int? StudentOf { get; set; }
        public string? Title { get; set; }
    }
}
