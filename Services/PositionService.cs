using AutoMapper;
using FinalProject.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Entities;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public class PositionService(OnlineCourseManagementDbContext context, IMapper mapper) : IPositionService
    {
        

        public async Task<PositionResponce> CreatePosition(AddPositionRequest request)
        {
            if (request == null)
                throw new Exception(nameof(request));

            if (await context.Positions.AnyAsync(u => u.PositionName == request.PositionName))
                throw new Exception($"User with username '{request.PositionName}' already exists");

            var position = mapper.Map<Position>(request);

            context.Positions.Add(position);
            await context.SaveChangesAsync();

            return mapper.Map<PositionResponce>(position);
        }

        public async Task DeleteUser(Guid id)
        {
            var position = await context.Positions.FindAsync(id)
                ?? throw new Exception($"User with id {id} not found");

            context.Positions.Remove(position);

            await context.SaveChangesAsync();
        }

        public async Task<List<PositionResponce>> GetAllPositions()
        {
            var positions = await context.Positions.ToListAsync();

            return mapper.Map<List<PositionResponce>>(positions);
        }

        public async Task<PositionResponce> GetPositionById(Guid id)
        {
            var position = await context.Positions
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception($"Position with id {id} not found");

            return mapper.Map<PositionResponce>(position);
        }

        
    }
}
