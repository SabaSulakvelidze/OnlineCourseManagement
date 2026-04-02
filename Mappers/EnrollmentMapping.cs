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
            CreateMap<LecturersCourse,LecturersCourseResponse>()
                .ForMember(
                    dest => dest.Course,
                    opt => opt.MapFrom(src => src.Course)
                )
                .ForMember(
                    dest => dest.Lecturer,
                    opt => opt.MapFrom(src => src.Lecturer)
                );

            CreateMap<StudentCourseRequest,StudentsCourse>();
            CreateMap<StudentsCourse,StudentsCourseResponse>()
                .ForMember(
                    dest => dest.Course,
                    opt => opt.MapFrom(src => src.Course)
                )
                .ForMember(
                    dest => dest.Student,
                    opt => opt.MapFrom(src => src.Student)
                );

        }
    }
}
