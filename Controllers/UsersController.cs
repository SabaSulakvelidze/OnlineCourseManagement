using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUsersService usersService,
        ICurrentUserService currentUserService) : ControllerBase
    {

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            return Ok(await usersService.GetUserById(id));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetAllUsers()
        {
            return Ok(await usersService.GetAllUsers());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id,UpdateUserRequest request)
        {
            return Ok(await usersService.UpdateUser(id, request));
        }

        

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();
            await usersService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("{userId}/profile-picture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            await usersService.UploadProfilePictureAsync(file);
            return Ok("Profile picture uploaded successfully");
        }

        [HttpGet("{userId}/profile-picture")]
        public async Task<ActionResult> GetProfilePicture(int userId)
        {
            var result = await usersService.GetProfilePictureAsync(userId);

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
            return Ok(await usersService.GetUsersByPosition(userPosition));
        }
       
    }
}
