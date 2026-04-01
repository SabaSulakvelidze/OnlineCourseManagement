using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController(
        IPositionService positionService,
        ICurrentUserService currentUserService
        ) : ControllerBase
    {
      
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddPosition(AddPositionRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.CreatePosition(request));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PositionResponse>> GetAllPositions()
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.GetAllPositions());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PositionResponse>> GetPositionById(Guid id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.GetPositionById(id));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<PositionResponse>> DeletePosition(Guid id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            await positionService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("AssignPossition")]
        [Authorize]
        public async Task<ActionResult> AssignPossition(UserPositionRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
            {
                return Forbid("You dont have permission for this action!");
            }

            await positionService.AssignPossition(request);
            return Ok("Position updated");

        }

        [HttpDelete("RemovePosition")]
        [Authorize]
        public async Task<ActionResult> RemovePosition(UserPositionRequest request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
            {
                return Forbid("You dont have permission for this action!");
            }

            await positionService.RemovePosition(request);
            return Ok();
        }
    }
}
