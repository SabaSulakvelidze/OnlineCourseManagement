using AutoMapper;
using OnlineCourseManagement.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourseManagement.Services
{
    public class PositionService(
        OnlineCourseManagementDbContext context,
        IMapper mapper) : IPositionService
    {
        public async Task AssignPossition(UserPositionRequest request)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
                throw new ElementNotFoundException($"User with id {request.UserId} was not found.");

            var positionExists = await context.Positions.AnyAsync(p => p.Id == request.PositionId);
            if (!positionExists)
                throw new ElementNotFoundException($"Position with id {request.PositionId} was not found.");

            var alreadyAssigned = await context.UsersPositions
                .AnyAsync(up => up.UsersId == request.UserId && up.PositionId == request.PositionId);

            if (alreadyAssigned)
                throw new ConflictException("This position is already assigned to the user.");

            context.UsersPositions.Add(new UsersPosition
            {
                Id = Guid.NewGuid(),
                UsersId = request.UserId,
                PositionId = request.PositionId
            });

            await context.SaveChangesAsync();
        }

        public async Task RemovePosition(UserPositionRequest request)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
                throw new ElementNotFoundException($"User with id {request.UserId} was not found.");

            var positionExists = await context.Positions.AnyAsync(p => p.Id == request.PositionId);
            if (!positionExists)
                throw new ElementNotFoundException($"Position with id {request.PositionId} was not found.");

            var userPosition = await context.UsersPositions
                .FirstOrDefaultAsync(up=> up.UsersId == request.UserId && up.PositionId == request.PositionId)
                ?? throw new ElementNotFoundException($"User with id {request.UserId} is not assigned to position with id {request.PositionId}");

            context.UsersPositions.Remove(userPosition);

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
