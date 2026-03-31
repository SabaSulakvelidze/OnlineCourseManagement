using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPositionService
    {
        Task<PositionResponse> CreatePosition(AddPositionRequest request);
        Task<List<PositionResponse>> GetAllPositions();
        Task<PositionResponse> GetPositionById(Guid id);
        Task RemovePosition(UserPositionRequest request);
        Task DeleteUser(Guid id);
        Task AssignPossition(UserPositionRequest request);


    }
}
