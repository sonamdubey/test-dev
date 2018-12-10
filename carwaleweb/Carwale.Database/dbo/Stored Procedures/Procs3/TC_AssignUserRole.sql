IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AssignUserRole]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AssignUserRole]
GO

	
CREATE PROCEDURE [dbo].[TC_AssignUserRole]
(
@RollId int,
@UserID int
)
as
update TC_Users set RoleId=@RollId where Id=@UserID 
