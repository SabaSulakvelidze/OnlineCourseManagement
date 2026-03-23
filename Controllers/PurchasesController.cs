using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Services;

namespace OnlineCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController(PurchaseService purchaseService) : ControllerBase
    {
        [HttpPost("buy")]
        public async Task<ActionResult<PurchaseResponse>> BuyCourse(BuyCourseRequest request)
        {
            var result = await purchaseService.BuyCourseAsync(request);
            return Ok(result);
        }
    }
}
