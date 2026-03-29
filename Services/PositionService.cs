using AutoMapper;
using OnlineCourseManagement.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using OnlineCourseManagement.Exceptions;

namespace OnlineCourseManagement.Services
{
    public class PositionService(OnlineCourseManagementDbContext context, IMapper mapper) : IPositionService
    {
        

        public async Task<PositionResponse> CreatePosition(ChangePosition request)
        {
            if (request == null)
                throw new Exception(nameof(request));

            if (await context.Positions.AnyAsync(u => u.PositionName == request.PositionName))
                throw new ConflictException($"User with username '{request.PositionName}' already exists");

            var position = mapper.Map<Position>(request);

            context.Positions.Add(position);
            await context.SaveChangesAsync();

            return mapper.Map<PositionResponse>(position);
        }

        public async Task DeleteUser(Guid id)
        {
            var position = await context.Positions.FindAsync(id)
                ?? throw new ElementNotFoundException($"User with id {id} not found");

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
