using AutoMapper;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class LectureMapping : Profile
    {
        public LectureMapping()
        {
            CreateMap<CreateLectureRequest, Lecture>();
            CreateMap<UpdateLectureRequest, Lecture>();
            CreateMap<Lecture, LectureResponse>()
                .ForMember(
                    dest => dest.Videos,
                    opt => opt.MapFrom(src => src.LectureVideos)
                );
        }
    }
}
