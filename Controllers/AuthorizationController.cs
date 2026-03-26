using FinalProject.Models.Requests;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly OnlineCourseManagementDbContext context;
        private readonly IConfiguration configuration;

        public AuthorizationController(OnlineCourseManagementDbContext context,IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        

        [HttpPost("api/Login")]

        public async Task<ActionResult> Login(AuthUser auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await context.Users
                .Include(u => u.UsersPositions)
                .ThenInclude(p => p.Position)
                .FirstOrDefaultAsync(
                item => item.Email == auth.Email 
                );

            if(result == null)
            {
                return BadRequest("ესეთი იუზერი არ არსებობ");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(auth.UserPassword, result.UserPassword);

            if (!isPasswordValid)
            {
                return BadRequest("Invalid password");
            }


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,result.Email),
                new Claim("UserId",result.Id.ToString())

            };

            var colection = result.UsersPositions.Select(item => item.Position.PositionName);
            foreach(var item in colection)
            {
                claims.Add(new Claim("Position", item));
            } 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(jwt);

        }



















    }
}
