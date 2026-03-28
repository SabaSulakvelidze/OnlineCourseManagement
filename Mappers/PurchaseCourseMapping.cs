using AutoMapper;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Mappers
{
    public class PurchaseCourseMapping : Profile
    {
        public PurchaseCourseMapping()
        {
            CreateMap<Purchase, PurchaseCourseResponse>();
        }
    }
}
