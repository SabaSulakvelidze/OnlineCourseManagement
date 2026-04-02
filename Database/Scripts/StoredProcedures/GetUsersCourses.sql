USE [OnlineCourseManagementDB]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetUsersCourses]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        u.Id,
        u.Username,
        lc.CourseId AS LecturerOf,
        sc.CourseId AS StudentOf,
        c.Title
    FROM Users AS u
    LEFT JOIN LecturersCourses AS lc ON u.Id = lc.LecturerId
    LEFT JOIN StudentsCourses AS sc ON u.Id = sc.StudentId
    JOIN Courses AS c ON c.Id = lc.CourseId OR c.Id = sc.CourseId
    WHERE u.Id = @UserId

END