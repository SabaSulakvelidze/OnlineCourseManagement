using AutoMapper;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class LectureVideoMapping : Profile
    {
        public LectureVideoMapping()
        {
            CreateMap<LectureVideo, LectureVideoResponse>();
        }
    }
}
