using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;

    }
}
