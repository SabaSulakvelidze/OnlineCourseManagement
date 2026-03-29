using System.ComponentModel.DataAnnotations;

namespace OnlineCourseManagement.Models.Requests
{
    public class UpdateUserRequest
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int PhoneNumber { get; set; }

    }
}
