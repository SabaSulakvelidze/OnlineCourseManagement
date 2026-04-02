USE [OnlineCourseManagementDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetUsersByPosition]
    @PositionName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.[Id]
      ,u.[Username]
      ,u.[Email]
      ,u.[PhoneNumber]
    FROM [OnlineCourseManagementDB].[dbo].[Users] AS u
    JOIN [OnlineCourseManagementDB].[dbo].[UsersPosition] AS up
    ON u.Id = up.UsersId
    JOIN [OnlineCourseManagementDB].[dbo].[Position] AS p
    ON up.PositionId = p.Id
    WHERE PositionName = @PositionName

END