IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetingRoles]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetingRoles]
GO
	
-- =============================================
-- Author:		Binumon George
-- Create date: 09-03-2012
-- Description:	Geting all roles under branch
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetingRoles]
	-- Add the parameters for the stored procedure here
	@UserId INT= NULL,
	@BranchId INT
AS
BEGIN
		DECLARE @IsSuperAdmin BIT
		SET @IsSuperAdmin=0
		--checking here user is super admin or not
		DECLARE @SuperAdminUserId INT
		
		IF(@UserId  IS NOT NULL )
			BEGIN
				SELECT TOP 1 @SuperAdminUserId=U.Id FROM TC_RoleTasks T 
				INNER JOIN TC_Roles R ON R.Id=T.RoleId 
				INNER JOIN TC_Users U ON R.Id=U.RoleId 
				WHERE T.TaskId=14 AND R.BranchId=@BranchId ORDER BY U.EntryDate ASC
	
	
				IF(@SuperAdminUserId =@UserId)
					BEGIN
						SET @IsSuperAdmin=1
					END
			END
			
		IF(@IsSuperAdmin !=1)
			BEGIN
				SELECT R.Id RoleId, R.RoleName , @IsSuperAdmin AS ISSuperAdmin FROM TC_Roles R WHERE R.IsActive=1 AND BranchId=@BranchId	
			END
			
END
