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
```

---

## Architecture Notes

The project follows a layered structure:

- **Controllers** handle HTTP requests and responses
- **Services** contain business logic
- **Models** contain entities, DTOs, enums, and stored procedure result models
- **Mappers** contain AutoMapper profiles
- **Exceptions** contain custom exception types for application-level error handling

### Implemented Architectural Elements

- Service layer abstraction using interfaces
- DTO-based request/response models
- AutoMapper profiles for model mapping
- Global exception handling with JSON error responses
- JWT-based authentication and authorization
- Static files support for simple local HTML pages

---

## Main Entities

- **User** — platform user with login and profile information
- **Position** — role/position such as Admin, Lecturer, or Student
- **UsersPosition** — join table between users and positions
- **Course** — course entity with description, price, and rating
- **Lecture** — lecture belonging to a course
- **LectureVideo** — uploaded video attached to a lecture
- **LecturersCourse** — relationship between lecturers and courses
- **StudentsCourse** — relationship between students and courses, including progress and grade
- **StudentLectureProgress** — tracks lecture completion progress per student
- **Purchase** — purchase record for course buying
- **Rating** — course rating and review by a user

### Stored Procedure Result Models

- **UsersByPosition**
- **UsersCourses**

---

## Stored Procedures

The project uses SQL stored procedures for some query scenarios.

### Create Stored Procedures

Stored procedure scripts are stored in:

Database/Scripts/StoredProcedures/GetUsersByPosition.sql

Database/Scripts/StoredProcedures/GetUsersCourses.sql

### `GetUsersByPosition`

Returns users filtered by a specific position.

### `GetUsersCourses`

Returns course relations for a given user, including lecturer and student associations.

These procedures return data into:
- `UsersByPosition`
- `UsersCourses`

---

## API Overview

### Authentication
- Register user
- Login user
- JWT token generation

### Users
- Manage users
- Retrieve current authenticated user
- Manage user profile-related information

### Positions
- Add and update positions
- Assign and remove positions for users

### Courses
- Create, update, delete, and retrieve courses
- Purchase and gift courses
- Rate and review courses

### Lectures
- Create, update, delete, and retrieve lectures
- Attach videos to lectures

### Enrollment
- Enroll and unenroll students
- Assign and unassign lecturers

### Videos
- Upload lecture videos using Cloudinary integration

---

## Setup and Run

### Prerequisites

- .NET SDK 10
- SQL Server LocalDB
- Visual Studio or another .NET-compatible IDE
- Cloudinary account for video uploads

### 1. Clone the Repository

```bash
git clone https://github.com/SabaSulakvelidze/OnlineCourseManagement.git
cd OnlineCourseManagement
```

### 2. Configure Database Connection

Before running migrations, update the SQL Server connection string as needed.

Also note that the scaffolded `DbContext` may contain a connection string in `OnConfiguring`, so make sure it matches your environment.

### 3. Run Migrations

Use EF Core commands:

```powershell
Add-Migration InitialCreate
Update-Database
```

### 4. Add Stored Procedure DbSets Manually

Because stored procedure result models are handled separately, add these lines manually to the `DbContext` if they are missing:

```csharp
public virtual DbSet<UsersByPosition> UsersByPosition { get; set; }
public virtual DbSet<UsersCourses> UsersCourses { get; set; }
```

### 5. Create Stored Procedures in SQL Server

Run the SQL scripts for:
- `GetUsersByPosition`
- `GetUsersCourses`

These must be created separately in the database.

### 6. Configure Application Settings

Update `appsettings.json` with your own values for:
- SQL Server connection string
- JWT settings
- Cloudinary settings

### 7. Run the Project

Run the application from Visual Studio or using:

```bash
dotnet run
```

---

## Launch Behavior

The project uses `launchSettings.json` to automatically open `dev-launch.html`, which redirects the browser to:
- Swagger UI
- AI-generated homepage or static viewer page

Default local URLs include:
- `http://localhost:5112`
- `https://localhost:7171`

---

## Swagger

Swagger is enabled in development mode and supports JWT Bearer authentication.

You can authorize requests in Swagger by providing a token in the following format:

```text
Bearer your_jwt_token
```

---

## Error Handling

The application uses global exception handling and returns JSON error responses.

### Custom Exception Mapping

- `ElementNotFoundException` → `404 Not Found`
- `ConflictException` → `409 Conflict`
- `ArgumentException` → `400 Bad Request`
- `UnauthorizedAccessException` → `401 Unauthorized`
- all other exceptions → `500 Internal Server Error`

Example response format:

```json
{
  "error": "Error message here"
}
```

---

## Static Files / Local UI Pages

The project includes local HTML pages under `wwwroot` for development and testing purposes, including:

- `dev-launch.html`
- `HomePage.html`
- `OnlineCourseManagementViewer.html`

These are used as lightweight front-end and testing helpers and are not meant to represent a full production front-end.

---

## Notes

- This project uses a mixed approach of scaffolded EF Core models and manual additions.
- Stored procedures are integrated separately through custom result models.
- LocalDB is used for development.
- Video handling is integrated through Cloudinary.
- The project is intended for learning, backend practice, and resume/portfolio presentation.

---

## Possible Future Improvements

- Add refresh token support
- Add unit and integration tests
- Improve deployment readiness
- Move secrets fully out of source configuration
- Add stricter validation and authorization policies
- Add a proper front-end client
- Improve migration and scaffolding consistency

---

## Repository

GitHub repository:

`https://github.com/SabaSulakvelidze/OnlineCourseManagement`
