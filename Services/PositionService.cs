using AutoMapper;
using OnlineCourseManagement.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Exceptions;

namespace OnlineCourseManagement.Services
{
    public class PositionService(OnlineCourseManagementDbContext context, IMapper mapper) : IPositionService
    {
        public async Task ChangePosition(ChangePositionRequest request)
        {
            var user = await context.Users
                .Include(u => u.UsersPositions)
                    .ThenInclude(up=> up.Position)
               .FirstOrDefaultAsync(u => u.Id == request.UsersId)
               ?? throw new ElementNotFoundException($"User with id {request.UsersId} not found");

            context.UsersPositions.RemoveRange(user.UsersPositions);

            var newUserPosition = new UsersPosition
            {
                UsersId = request.UsersId,
                PositionId = request.PositionId
            };

            await context.UsersPositions.AddAsync(newUserPosition);
            await context.SaveChangesAsync();
        }

        public async Task<PositionResponse> CreatePosition(AddPositionRequest request)
        {
            if (await context.Positions.AnyAsync(u => u.PositionName == request.PositionName))
                throw new ConflictException($"Position with PositionName '{request.PositionName}' already exists");

            var position = mapper.Map<Position>(request);

            context.Positions.Add(position);
            await context.SaveChangesAsync();

            return mapper.Map<PositionResponse>(position);
        }

        public async Task DeleteUser(Guid id)
        {
            var position = await context.Positions.FindAsync(id)
                ?? throw new ElementNotFoundException($"Position with id {id} not found");

            context.Positions.Remove(position);

            await context.SaveChangesAsync();
        }

        public async Task<List<PositionResponse>> GetAllPositions()
        {
            var positions = await context.Positions.ToListAsync();

            return mapper.Map<List<PositionResponse>>(positions);
        }

        public async Task<PositionResponse> GetPositionById(Guid id)
        {
            var position = await context.Positions
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new ElementNotFoundException($"Position with id {id} not found");

            return mapper.Map<PositionResponse>(position);
        }

        
    }
}
