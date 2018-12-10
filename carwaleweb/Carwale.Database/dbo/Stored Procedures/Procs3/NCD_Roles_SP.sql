IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_Roles_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_Roles_SP]
GO

	

-- =============================================
-- Author:		Umesh Ojha
-- Create date: 18-10-2011
-- Description:	Role details of under a dealer.
-- =============================================
CREATE PROCEDURE [dbo].[NCD_Roles_SP] 
	-- Add the parameters for the stored procedure here
	@Id INT=NULL,
	@RoleName VARCHAR(50),
	@RoleDescription VARCHAR(100),
	@Status INT OUTPUT,
	@DealerId INT,
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
			IF NOT EXISTS(SELECT top 1 Id FROM NCD_Roles WHERE RoleName = @RoleName AND DealerId= @DealerId )
				BEGIN
					IF NOT EXISTS(SELECT top 1 Id FROM NCD_Roles WHERE  TaskSet=@Taskset AND DealerId= @DealerId)
						BEGIN
							INSERT INTO NCD_Roles(RoleName, RoleDescription, RoleCreationDate,TaskSet,DealerId,IsActive)
							VALUES(@RoleName, @RoleDescription, GETDATE(),@Taskset,@DealerId,1)
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
	Else --IF id contaiing data, we updatig dealer information
		BEGIN
							-- checking Rollname  available or not.if not inserting data
			IF NOT EXISTS(SELECT top 1 Id FROM NCD_Roles WHERE RoleName = @RoleName AND DealerId= @DealerId AND Id <> @Id )
				BEGIN
					UPDATE NCD_Roles SET RoleName=@RoleName, RoleDescription=@RoleDescription,TaskSet=@Taskset WHERE Id = @Id
					SET @Status=4			
				END
		END
END

