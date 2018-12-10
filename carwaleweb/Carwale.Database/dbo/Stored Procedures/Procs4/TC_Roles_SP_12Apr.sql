IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Roles_SP_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Roles_SP_12Apr]
GO

	
-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- =============================================
-- Author:		Binumon George
-- Create date: 13-06-2011
-- Description:	Role details of under a dealer.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Roles_SP_12Apr] 
	-- Add the parameters for the stored procedure here
	@Id INT=NULL,
	@RoleName VARCHAR(50),
	@RoleDescription VARCHAR(100),
	@Status INT OUTPUT,
	@BranchId INT,
	@Taskset VARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
	IF(@Id IS NULL)--IF id parameter is null, we inserting new user to TC_Users table
		BEGIN
			-- checking Rollname  available or not.if not inserting data
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Roles WHERE RoleName = @RoleName AND BranchId= @BranchId AND IsActive = 1)
				BEGIN
					-- 
					IF NOT EXISTS(SELECT top 1 Id FROM TC_Roles WHERE TaskSet=@Taskset AND BranchId= @BranchId)
						BEGIN
							INSERT INTO TC_Roles(RoleName, RoleDescription, RoleCreationDate,TaskSet,BranchId)
							VALUES(@RoleName, @RoleDescription, GETDATE(),@Taskset,@BranchId)
							SET @Status=1
						END
					ELSE
						BEGIN
							SET @Status=2
						END
				 END
			   ELSE
				BEGIN
					SET @Status=3
				END
		END
	Else --IF id contaiing data, we updatig branch information
		BEGIN
							-- checking Rollname  available or not.if not inserting data
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Roles WHERE RoleName = @RoleName AND BranchId= @BranchId AND Id <> @Id )
				BEGIN
					UPDATE TC_Roles SET RoleName=@RoleName, RoleDescription=@RoleDescription,TaskSet=@Taskset,ModifiedDate=GETDATE() WHERE Id = @Id
					SET @Status=4
				END
		END
END







