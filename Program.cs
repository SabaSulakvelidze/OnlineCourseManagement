using FinalProject.Mappers;
using Microsoft.EntityFrameworkCore;
using OnlineCourseManagement.Models;
using OnlineCourseManagement.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddDbContext<OnlineCourseManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserMapping));


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
