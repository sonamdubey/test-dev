IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_IsRoleSuperAdmin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_IsRoleSuperAdmin]
GO

	
-- Author:		Binumon George
-- Create date: 10-04-2012
-- Description:	Finding user super admin or not
-- =============================================
CREATE PROCEDURE [dbo].[TC_IsRoleSuperAdmin]	
	@RoleId INT,
	@BranchId BIGINT,
	@IsSuperAdmin BIT OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from	
	SET NOCOUNT ON;

	SET @IsSuperAdmin=0	
	DECLARE @SuperAdminRoleId INT
	
	SELECT TOP 1 @SuperAdminRoleId=R.Id FROM TC_RoleTasks T 
				INNER JOIN TC_Roles R ON R.Id=T.RoleId  
				WHERE T.TaskId=14 AND R.BranchId=@BranchId ORDER BY R.RoleCreationDate ASC
	
	IF(@RoleId=@SuperAdminRoleId)
	BEGIN
		SET @IsSuperAdmin=1
	END
END



