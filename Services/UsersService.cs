

using AutoMapper;
using FinalProject.Models.Requests;
using FinalProject.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Responses;

namespace OnlineCourseManagement.Service
{
    public class UsersService(
        OnlineCourseManagementDbContext context,
        IMapper mapper) : IUsersService
    {

        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private static readonly string[] AllowedContentTypes =
        [
            "image/jpeg",
            "image/png",
            "image/webp"
        ];

        private const long MaxFileSize = 2 * 1024 * 1024; // 2MB

        public async Task<UserResponse> CreateUser(CreateUserRequest request)
        {
            if (request == null)
                throw new Exception(nameof(request));

            if (await context.Users.AnyAsync(u => u.Username == request.Username))
                throw new Exception($"User with username '{request.Username}' already exists");

            var user = mapper.Map<User>(request);

            var defaultImagePath = Path.Combine("Assets", "defaultProfilePicture.png");
            var imageBytes = await File.ReadAllBytesAsync(defaultImagePath);

            user.ProfileImage = imageBytes;
            user.ProfileImageFileName = "defaultProfilePicture.png";
            user.ProfileImageContentType = "image/png";

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return mapper.Map<UserResponse>(user);
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var users = await context.Users.ToListAsync();

            return mapper.Map<List<UserResponse>>(users);
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception($"User with id {id} not found");

            return mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await context.Users.FindAsync(id)
                ?? throw new Exception($"User with id {id} not found");

            mapper.Map(request, user);

            await context.SaveChangesAsync();

            return mapper.Map<UserResponse>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id)
                ?? throw new Exception($"User with id {id} not found");

            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var user = await context.Users.FindAsync(userId)
                 ?? throw new Exception($"User with id {userId} not found");

            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            if (file.Length > MaxFileSize)
                throw new Exception("File size must not exceed 2MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new Exception("Only jpg, jpeg, png, and webp files are allowed");

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new Exception("Invalid image content type");

            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            user.ProfileImage = imageBytes;
            user.ProfileImageFileName = uniqueFileName;
            user.ProfileImageContentType = file.ContentType;

            await context.SaveChangesAsync();
        }

        public async Task<UserProfileImageResponse?> GetProfilePictureAsync(int userId)
        {
            var user = await context.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.ProfileImage == null || string.IsNullOrWhiteSpace(user.ProfileImageContentType))
                return null;

            return new UserProfileImageResponse
            {
                ImageBytes = user.ProfileImage,
                ContentType = user.ProfileImageContentType,
                FileName = user.ProfileImageFileName
            }; ;
        }
    }
}
