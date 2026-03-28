using AutoMapper;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.Identity.Client;
using OnlineCourseManagement.Models.Entities;

namespace FinalProject.Mappers
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
