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

        public async Task<ActionResult> AddPosition(ChangePosition request)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.CreatePosition(request));
        }

        [HttpGet]
        public async Task<ActionResult<PositionResponse>> GetAllPositions()
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.GetAllPositions());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PositionResponse>> GetUserById(Guid id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            return Ok(await positionService.GetPositionById(id));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<PositionResponse>> DeleteUser(Guid id)
        {
            var positions = currentUserService.UserPositions;

            if (!positions.Contains("Admin"))
                return Forbid();

            await positionService.DeleteUser(id);
            return Ok();
        }


        //[HttpPost("Change Position")]
        //public async Task<ActionResult> ChangePosition(ChangePosition request)
        //{




        //    var permision = User.Claims.Where(item => item.Type == "Position").Select(item => item.Value).ToList();



        //    if (!permision.Contains("Admin"))
        //    {
        //        return Unauthorized("You have not permision to change");
        //    }

        //    return Ok("done!!!!");
        //    //Position position = new Position();
        //    //position.PositionName = request.PositionName;

        //    //context.Positions.Add(position);

        //    //await context.SaveChangesAsync();

        //    //return Ok(position);





        //}
    }
}
