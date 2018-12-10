IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_IsSuperAdmin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_IsSuperAdmin]
GO

	-- Author:		Binumon George
-- Create date: 07-03-2012
-- Modified By: Surendar on 13 march for changing roles table
-- Description:	Finding user super admin or not
-- [dbo].[TC_UsersDelete] 1,5,@Status
-- =============================================
CREATE  PROCEDURE       [dbo].[TC_IsSuperAdmin]	
	@UserId INT,
	@BranchId BIGINT,
	@IsSuperAdmin BIT OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	SET NOCOUNT ON;

	SET @IsSuperAdmin=0	
	DECLARE @SuperAdminUserId INT
	
/* 15-03-2013	SELECT TOP 1 @SuperAdminUserId=U.Id FROM TC_RoleTasks T 
				INNER JOIN TC_Roles R ON R.Id=T.RoleId 
				INNER JOIN TC_Users U ON R.Id=U.RoleId 
				WHERE T.TaskId=14 AND R.BranchId=@BranchId ORDER BY U.EntryDate ASC    15-03-2013 */
				
				SELECT TOP 1 @SuperAdminUserId=U.Id 
				   FROM TC_UsersRole AS T
				   JOIN TC_Users AS U ON U.Id=T.UserId        
					WHERE T.RoleId=9 
					 AND U.BranchId=@BranchId ORDER BY U.EntryDate ASC 
	
	
	IF(@UserId=@SuperAdminUserId)
	BEGIN
		SET @IsSuperAdmin=1
	END
END
