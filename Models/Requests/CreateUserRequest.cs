using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage ="This bar can not be empty")]
        [StringLength(50,MinimumLength =2)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage ="Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="Password is required")]
        public string UserPassword { get; set; } = null!;

        [Required(ErrorMessage ="Please enter your phone number")]
        public int PhoneNumber { get; set; }

    }
}
