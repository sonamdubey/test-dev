IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteRole_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteRole_SP]
GO

	-- =============================================
-- Modified by:		Binumon George
-- Create date: 09-03-2012
-- Description:	dont allow to delete user if it super Admin
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 18-07-2011
-- Description:	Deleting role if role not assigned to any active users.
-- =============================================
	CREATE PROCEDURE [dbo].[TC_DeleteRole_SP] 
	-- Add the parameters for the stored procedure here
	@Id  SMALLINT,
	@Status INT OUTPUT,
	@BranchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @IsSuperAdmin BIT
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
	IF NOT EXISTS(SELECT top 1 Id FROM TC_Users WHERE RoleId = @Id AND IsActive=1)
		BEGIN
			EXEC TC_IsRoleSuperAdmin @Id, @BranchId, @IsSuperAdmin OUTPUT 
			IF(@IsSuperAdmin !=1)
				BEGIN
					UPDATE TC_Roles set IsActive=0 WHERE Id= @Id
					SET @Status=1
				END
			ELSE
				BEGIN
					SET @Status=2
				END
		END
END


