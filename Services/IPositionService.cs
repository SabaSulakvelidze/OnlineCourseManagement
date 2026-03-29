using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IPositionService
    {
        Task<PositionResponce> CreatePosition( AddPositionRequest request);

        Task<List<PositionResponce>> GetAllPositions();

        Task<PositionResponce> GetPositionById(Guid id);


        Task DeleteUser(Guid id);

       
    }
}
