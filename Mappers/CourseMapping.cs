using AutoMapper;
using OnlineCourseManagement.Models;
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
            CreateMap<RateCourseRequest, Rating>();
            CreateMap<Rating, RatingResponse>();
        }
    }
}
