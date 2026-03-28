using AutoMapper;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class EnrollmentMapping : Profile
    {
        public EnrollmentMapping()
        {
            CreateMap<AssignLecturerRequest,LecturersCourse>();
            CreateMap<LecturersCourse,LecturersCourseResponse>();

            CreateMap<EnrollStudentRequest,StudentsCourse>();
            CreateMap<StudentsCourse,StudentsCourseResponse>();

        }
    }
}
