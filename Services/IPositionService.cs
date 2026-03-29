using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPositionService
    {
        Task<PositionResponse> CreatePosition( ChangePosition request);

        Task<List<PositionResponse>> GetAllPositions();

        Task<PositionResponse> GetPositionById(Guid id);


        Task DeleteUser(Guid id);

       
    }
}
