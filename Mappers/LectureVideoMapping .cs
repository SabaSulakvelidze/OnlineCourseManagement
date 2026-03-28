using AutoMapper;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class LectureVideoMapping : Profile
    {
        public LectureVideoMapping()
        {
            CreateMap<AddLectureVideoRequest, LectureVideo>();
            CreateMap<LectureVideo, LectureVideoResponse>();
        }
    }
}
