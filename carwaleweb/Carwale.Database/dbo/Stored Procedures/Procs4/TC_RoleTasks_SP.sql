IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RoleTasks_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RoleTasks_SP]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 14-06-2011
-- Description:	Combination of Role and task
-- =============================================
CREATE PROCEDURE [dbo].[TC_RoleTasks_SP] 
	-- Add the parameters for the stored procedure here
	@TaskId  SMALLINT ,
	@UserRoleId INT,
	@Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
			IF NOT EXISTS(SELECT top 1 TaskId FROM TC_RoleTasks WHERE UserRoleId = @UserRoleId AND TaskId = @TaskId)
				BEGIN
					INSERT INTO TC_RoleTasks(UserRoleId, TaskId)
					VALUES(@UserRoleId, @TaskId)
					set @Status=1
				END
END

