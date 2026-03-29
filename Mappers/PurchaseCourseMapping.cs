using AutoMapper;
using OnlineCourseManagement.Models;
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
