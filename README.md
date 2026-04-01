# OnlineCourseManagement

OnlineCourseManagement is a practice and portfolio ASP.NET Core Web API project for managing an online course platform. It was built to demonstrate backend development skills in .NET, database design with SQL Server, JWT-based authentication, role-based authorization, file/video integration, and stored procedure usage.

## Project Goal

This project simulates an online learning platform where users can register, log in, manage courses and lectures, enroll in courses, assign lecturers, purchase and gift courses, rate courses, and track lecture progress.

It was created primarily for learning and resume purposes, with emphasis on:
- ASP.NET Core Web API development
- Entity Framework Core
- SQL Server and stored procedures
- JWT authentication and authorization
- AutoMapper-based DTO mapping
- Cloudinary integration for video storage

---

## Tech Stack

- **Framework:** ASP.NET Core Web API (.NET 10)
- **Database:** SQL Server LocalDB
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Authentication
- **Object Mapping:** AutoMapper
- **Password Hashing:** BCrypt.Net-Next
- **Video Storage:** Cloudinary
- **API Documentation:** Swagger / Swashbuckle

### Main Packages

- AutoMapper
- BCrypt.Net-Next
- CloudinaryDotNet
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore

---

## Features

All of the following features are implemented:

- User registration
- User login
- Current authenticated user profile retrieval
- User create/update/delete operations
- Position assignment and removal
- Course create/update/delete operations
- Lecture create/update/delete operations
- Lecture video upload
- Student enrollment and unenrollment
- Lecturer assignment and unassignment
- Course purchase
- Course gifting
- Course rating and review
- Retrieve users by position using stored procedure
- Retrieve user course relations using stored procedure

---

## Roles and Authorization

The system uses position-based authorization with three main roles stored in the database:

- **Admin**
  - full access to management operations
  - can manage users, positions, courses, and assignments

- **Lecturer**
  - can manage assigned course and lecture content
  - cannot perform admin-only operations

- **Student**
  - can browse, purchase, enroll in, and rate courses
  - can view their own course progress
  - cannot access admin or lecturer management operations

Authentication is implemented using **JWT Bearer tokens**.

---

## Project Structure

```text
OnlineCourseManagement/
├── Controllers/
│   ├── AuthorizationController.cs
│   ├── CoursesController.cs
│   ├── EnrollmentController.cs
│   ├── LecturesController.cs
│   ├── PositionController.cs
│   ├── UsersController.cs
│   └── VideosController.cs
│
├── Services/
│   ├── UsersService.cs
│   ├── CourseService.cs
│   ├── LectureService.cs
│   ├── EnrollmentServices.cs
│   ├── PositionService.cs
│   ├── CurrentUserService.cs
│   ├── FakePaymentGateway.cs
│   └── CloudinaryVideoStorageService.cs
│
├── Models/
│   ├── Requests/
│   ├── Responses/
│   ├── Procedures/
│   ├── Enums/
│   └── CloudeStorage/
│
├── Mappers/
├── Exceptions/
├── Assets/
├── wwwroot/
├── Properties/
├── Program.cs
├── appsettings.json
└── README.md