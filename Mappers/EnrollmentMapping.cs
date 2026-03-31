using AutoMapper;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class EnrollmentMapping : Profile
    {
        public EnrollmentMapping()
        {
            CreateMap<LecturerCourseRequest,LecturersCourse>();
            CreateMap<LecturersCourse,LecturersCourseResponse>();

            CreateMap<StudentCourseRequest,StudentsCourse>();
            CreateMap<StudentsCourse,StudentsCourseResponse>();

        }
    }
}
