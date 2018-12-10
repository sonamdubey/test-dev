IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UsersDelete_24SEP2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UsersDelete_24SEP2012]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 07-03-2012
-- Description:	User Deleting on bsis of user Id
-- [dbo].[TC_UsersDelete] 1,5,@Status
-- =============================================
CREATE PROCEDURE [dbo].[TC_UsersDelete_24SEP2012]
	-- Add the parameters for the stored procedure here
	@UserId INT,
	@BranchId INT,
	@Status INT OUTPUT
AS
BEGIN
		--checking here user is super admin or not
		--EXEC TC_IsSuperAdmin @RoleId, @BranchId, @IsSuperAdmin OUTPUT

		--if not a super admin deleting his details
		DECLARE @SuperAdminUserId INT
		SET @SuperAdminUserId=NULL
		SET @Status=0
		
		--checking here param userid superadmin or not
		SELECT @SuperAdminUserId=Id FROM
		(SELECT U.Id, ROW_NUMBER() OVER (ORDER BY U.EntryDate ASC) AS RowN
		FROM TC_Users U INNER JOIN TC_Roles Rls
		ON  U.RoleId=Rls.Id WHERE U.BranchId=@BranchId 
		AND Rls.TaskSet LIKE '%,22,%')a WHERE RowN=1
		
		IF (@SuperAdminUserId != @UserId)--checking here basic super adminor not
			BEGIN
				UPDATE TC_Users SET IsActive=0 WHERE Id=@UserId AND BranchId=@BranchId
				SET @Status=1
			END	
			
END