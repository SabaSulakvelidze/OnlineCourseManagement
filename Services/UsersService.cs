
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourseManagement.Exceptions;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Models.Procedures;
using OnlineCourseManagement.Models.Requests;
using OnlineCourseManagement.Models.Responses;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineCourseManagement.Services
{
    public class UsersService(
        OnlineCourseManagementDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        ICurrentUserService currentUserService) : IUsersService
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
                throw new ConflictException($"User with username '{request.Username}' already exists");

            var user = mapper.Map<User>(request);

            user.UserPassword = BCrypt.Net.BCrypt.HashPassword(request.UserPassword);

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
                ?? throw new ElementNotFoundException($"User with id {id} not found");

            return mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await context.Users.FindAsync(id)
                ?? throw new ElementNotFoundException($"User with id {id} not found");



            mapper.Map(request, user);

            await context.SaveChangesAsync();

            return mapper.Map<UserResponse>(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id)
                ?? throw new ElementNotFoundException($"User with id {id} not found");

            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task UploadProfilePictureAsync(IFormFile file)
        {
            var currentUserId = currentUserService.UserId;
            var user = await context.Users.FindAsync(currentUserId)
                 ?? throw new ElementNotFoundException($"User with id {currentUserId} not found");

            if (file == null || file.Length == 0)
                throw new ConflictException("File is empty");

            if (file.Length > MaxFileSize)
                throw new ConflictException("File size must not exceed 2MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new ConflictException("Only jpg, jpeg, png, and webp files are allowed");

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ConflictException("Invalid image content type");

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
            };
        }

        public async Task<List<UsersByPosition>> GetUsersByPosition(string positionName)
        {
            var sqlParams = new SqlParameter("@PositionName", positionName);
            var result = await context.Set<UsersByPosition>().FromSqlRaw("EXEC GetUsersByPosition @PositionName", sqlParams).ToListAsync();

            return result;
        }

        public async Task<string> Login(AuthUser auth)
        {
            var result = await context.Users
                .Include(u => u.UsersPositions)
                .ThenInclude(p => p.Position)
                .FirstOrDefaultAsync(
                item => item.Email == auth.Email)
                ?? throw new UnauthorizedAccessException("Email or Password is incorrect!");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(auth.UserPassword, result.UserPassword);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Email or Password is incorrect!");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,result.Email),
                new Claim("UserId",result.Id.ToString())

            };

            var colection = result.UsersPositions.Select(item => item.Position.PositionName);
            foreach (var item in colection)
            {
                claims.Add(new Claim("Position", item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<CurrentUserResponse> GetCurrentUser()
        {
            var currentUser = await context.Users
                .Include(u => u.LecturersCourses)
                    .ThenInclude(sc => sc.Course)
                .Include(u => u.StudentsCourses)
                    .ThenInclude(sc=>sc.Course)
                .Include(u => u.StudentLectureProgresses)
                .Include(u => u.UsersPositions)
                    .ThenInclude(up => up.Position)
                .FirstOrDefaultAsync(u => u.Id == currentUserService.UserId)
                ?? throw new ElementNotFoundException("Warning: Current user was not found!");

            return new CurrentUserResponse
            {
                Id = currentUser.Id,
                Username = currentUser.Username,
                ProfileImage = currentUser.ProfileImage,
                ProfileImageFileName = currentUser.ProfileImageFileName,
                ProfileImageContentType = currentUser.ProfileImageContentType,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                LecturersCourses = mapper.Map<List<LecturersCourseResponse>>(currentUser.LecturersCourses),
                StudentsCourses = mapper.Map<List<StudentsCourseResponse>>(currentUser.StudentsCourses),
                UsersPositions = currentUser.UsersPositions.Select(up => up.Position.PositionName).ToList()
            };
        }
    }
}
