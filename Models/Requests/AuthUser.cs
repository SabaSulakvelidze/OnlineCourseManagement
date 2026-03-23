using System.ComponentModel.DataAnnotations;

namespace OnlineCourseManagement.Models.Requests
{
    public class AuthUser
    {
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; } = null!;
    }
}
