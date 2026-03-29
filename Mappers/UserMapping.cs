using AutoMapper;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using Microsoft.Identity.Client;
using OnlineCourseManagement.Models;

namespace OnlineCourseManagement.Mappers
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
