using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Service;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IUsersService userService,
        ICurrentUserService currentUserService) : ControllerBase
    {
    
        [HttpPost("/api/Register")]
        public async Task<ActionResult<UserResponse>> Register(CreateUserRequest request)
        {
            return Ok(await userService.CreateUser(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            return Ok(await userService.GetUserById(id));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UpdateUserRequest request)
        {
            return Ok(await userService.UpdateUser(id, request));
        }

        

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();
            await userService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("{userId}/profile-picture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(int userId, IFormFile file)
        {
            await userService.UploadProfilePictureAsync(userId, file);
            return Ok("Profile picture uploaded successfully");
        }

        [HttpGet("{userId}/profile-picture")]
        [Authorize]
        public async Task<ActionResult> GetProfilePicture(int userId)
        {
            var result = await userService.GetProfilePictureAsync(userId);

            if (result == null)
                return NotFound("Profile picture not found");

            return File(result.ImageBytes, result.ContentType);
        }

        [HttpGet("getUsersByPosition")]
        [Authorize]
        public async Task<ActionResult<UsersByPosition>> GetUsersByPosition([FromQuery] string userPosition)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();
            return Ok(await userService.GetUsersByPosition(userPosition));
        }
       
    }
}
