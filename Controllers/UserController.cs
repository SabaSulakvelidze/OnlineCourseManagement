using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Service;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUsersService userService) : ControllerBase
    {
    
        [HttpPost("/api/Register")]
        public async Task<ActionResult<UserResponse>> Register(CreateUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request == null)
                return BadRequest(request);

            return Ok(await userService.CreateUser(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            return Ok(await userService.GetUserById(id));
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UpdateUserRequest request)
        {
            if (request == null)
                return BadRequest(request);

            return Ok(await userService.UpdateUser(id, request));
        }

        

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            await userService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("{userId}/profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(int userId, IFormFile file)
        {
            await userService.UploadProfilePictureAsync(userId, file);
            return Ok("Profile picture uploaded successfully");
        }

        [HttpGet("{userId}/profile-picture")]
        public async Task<IActionResult> GetProfilePicture(int userId)
        {
            var result = await userService.GetProfilePictureAsync(userId);

            if (result == null)
                return NotFound("Profile picture not found");

            return File(result.ImageBytes, result.ContentType);
        }
    }
}
