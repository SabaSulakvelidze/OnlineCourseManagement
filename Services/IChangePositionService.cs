using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;

namespace OnlineCourseManagement.Services
{
    public interface IChangePositionService
    {
        public Task ChangePosition(ChangePositionRequest request);
       
    }
}
