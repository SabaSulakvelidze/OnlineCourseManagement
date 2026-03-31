using Microsoft.AspNetCore.Mvc;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Services
{
    public interface IUsersService
    {
        Task<UserResponse> CreateUser(CreateUserRequest request);

        Task<List<UserResponse>> GetAllUsers();

        Task<UserResponse> GetUserById(int id);

        Task<UserResponse> UpdateUser(int id, UpdateUserRequest request);

        Task DeleteUser(int id);

        Task UploadProfilePictureAsync(int userId, IFormFile file);

        Task<UserProfileImageResponse?> GetProfilePictureAsync(int userId);

        Task<List<UsersByPosition>> GetUsersByPosition(string positionName);

        Task<string> Login(AuthUser auth);

    }
}
