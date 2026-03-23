using AutoMapper;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class PositionMapping : Profile
    {
        public PositionMapping()
        {
            CreateMap<ChangePosition, Position>();
            CreateMap<UpdatePositionRequest, Position>();
            CreateMap<Position, PositionResponce>();
        }
    }
}
