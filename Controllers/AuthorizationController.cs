using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(IUsersService usersService) : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<ActionResult> Login(AuthUser auth)
        {
            return Ok(await usersService.Login(auth));

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserResponse>> Register(CreateUserRequest request)
        {
            return Ok(await usersService.CreateUser(request));
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<CurrentUserResponse>> GetCurrentUser()
        {
            return Ok(await usersService.GetCurrentUser());
        }



    }
}
