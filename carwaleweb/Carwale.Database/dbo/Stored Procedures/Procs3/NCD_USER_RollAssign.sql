IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_USER_RollAssign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_USER_RollAssign]
GO

	
Create procedure [dbo].[NCD_USER_RollAssign]
(
@RollId int,
@UserID int
)
as
update NCD_Users set RoleId=@RollId where Id=@UserID 
