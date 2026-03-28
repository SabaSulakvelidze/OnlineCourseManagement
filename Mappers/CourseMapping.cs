using AutoMapper;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<CreateCourseRequest, Course>();
            CreateMap<UpdateCourseRequest, Course>();
            CreateMap<Course, CourseResponse>();
        }
    }
}
