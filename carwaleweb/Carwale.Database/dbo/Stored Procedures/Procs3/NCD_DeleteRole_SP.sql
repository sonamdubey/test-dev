IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_DeleteRole_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_DeleteRole_SP]
GO

	

-- =============================================
-- Author:		Umesh
-- Create date: 18-10-2011
-- Description:	Deleting role if role not assigned to any active users.
-- =============================================
	CREATE PROCEDURE [dbo].[NCD_DeleteRole_SP] 
	-- Add the parameters for the stored procedure here
	@Id  SMALLINT,
	@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
 		BEGIN
			IF NOT EXISTS(SELECT top 1 Id FROM NCD_Users WHERE RoleId = @Id AND IsActive=1)
				BEGIN
					UPDATE NCD_Roles set IsActive=0 WHERE Id= @Id
					SET @Status=1
				END
		END
		
END


