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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPosition(AddPositionRequest request)
        {
            return Ok(await positionService.CreatePosition(request));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PositionResponse>> GetAllPositions()
        {
            return Ok(await positionService.GetAllPositions());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PositionResponse>> GetPositionById(Guid id)
        {
            return Ok(await positionService.GetPositionById(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PositionResponse>> DeletePosition(Guid id)
        {
            await positionService.DeleteUser(id);
            return Ok();
        }

        [HttpPost("AssignPossition")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignPossition([FromQuery] UserPositionRequest request)
        {
            await positionService.AssignPossition(request);
            return Ok("Position updated");

        }

        [HttpDelete("RemovePosition")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemovePosition([FromQuery] UserPositionRequest request)
        {
            await positionService.RemovePosition(request);
            return Ok();
        }
    }
}
