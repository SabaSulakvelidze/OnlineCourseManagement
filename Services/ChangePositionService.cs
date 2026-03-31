using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;

namespace OnlineCourseManagement.Services
{
    public class ChangePositionService(OnlineCourseManagementDbContext context,IMapper mapper):IChangePositionService
    {
        public async Task ChangePosition(ChangePositionRequest request)
        {
            var user = await context.Users
                .Include(u => u.UsersPositions)
                .FirstOrDefaultAsync(u => u.Id == request.UsersId);

            if (user == null)
                throw new Exception("User not found");

            
            context.UsersPositions.RemoveRange(user.UsersPositions);


            var newUserPosition = new UsersPosition
            {
                UsersId = request.UsersId,
                PositionId = request.PositionId
            };

            await context.UsersPositions.AddAsync(newUserPosition);
            await context.SaveChangesAsync();
        }
    }
}
