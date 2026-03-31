using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPositionService
    {
        Task<PositionResponse> CreatePosition( AddPositionRequest request);

        Task<List<PositionResponse>> GetAllPositions();

        Task<PositionResponse> GetPositionById(Guid id);

        Task ChangePosition(ChangePositionRequest request);
        Task DeleteUser(Guid id);

       
    }
}
