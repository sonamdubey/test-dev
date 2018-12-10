IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Roles_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Roles_SP]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 04-04-2012
-- Description:	Adding and updating combination of roleid and taskid in tc_roletask
-- =============================================
-- ModifiedBy:		Binumon George
-- Modified date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- =============================================
-- Author:		Binumon George
-- Create date: 13-06-2011
-- Description:	Role details of under a dealer.
-- Modified By: Nilesh Utture on 26th December, 2012 Added IsActive=1 in case of update
-- =============================================
CREATE PROCEDURE [dbo].[TC_Roles_SP] 
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
	DECLARE @Separator_position INT -- This is used to locate each separator character  
    DECLARE @array_value VARCHAR(1000)
	DECLARE  @Separator CHAR(1)=','
	--SET @Taskset = @Taskset + @Separator 
	DECLARE @RoleId BIGINT
    -- Insert statements for procedure here
	IF(@Id IS NULL)--IF id parameter is null, we inserting new user to TC_Users table
		BEGIN
			-- checking Rollname  available or not.if not inserting data
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Roles WHERE RoleName = @RoleName AND BranchId= @BranchId AND IsActive = 1)
				BEGIN
					INSERT INTO TC_Roles(RoleName, RoleDescription, RoleCreationDate,BranchId)
					VALUES(@RoleName, @RoleDescription, GETDATE(),@BranchId)
					SET @RoleId=SCOPE_IDENTITY()
					 -- Loop through the string searching for separtor characters    
					WHILE PATINDEX('%' + @Separator + '%', @Taskset) <> 0   
						BEGIN  			
							 -- patindex matches the a pattern against a string  
							SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@Taskset)  
							SELECT  @array_value = LEFT(@Taskset, @Separator_position - 1) 
							INSERT INTO TC_RoleTasks (RoleId,TaskId)VALUES(@RoleId,@array_value)
							SELECT  @Taskset = STUFF(@Taskset, 1, @Separator_position, '')  
						END--while End
						SET @Status=1--successfully saved		
				END
				ELSE
				BEGIN
					SET @Status=2--Role Name already Exist
				END	
		END
		ELSE --IF id contaiing data, we updatig role information
		BEGIN
			-- checking Rollname  available or not.if not updating data
			-- Nilesh Utture on 26th December, 2012 Added condition IsActive = 1
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Roles WHERE RoleName = @RoleName AND BranchId= @BranchId AND Id <> @Id AND IsActive=1)
				BEGIN
					UPDATE TC_Roles SET RoleName=@RoleName, RoleDescription=@RoleDescription,ModifiedDate=GETDATE() WHERE Id = @Id
					DELETE TC_RoleTasks WHERE RoleId=@Id--first deleting existing task related to role
					WHILE PATINDEX('%' + @Separator + '%', @Taskset) <> 0   
						BEGIN  			
							 -- patindex matches the a pattern against a string  
							SELECT  @Separator_position = PATINDEX('%' + @Separator + '%',@Taskset)  
							SELECT  @array_value = LEFT(@Taskset, @Separator_position - 1) 
							INSERT INTO TC_RoleTasks (RoleId,TaskId)VALUES(@Id,@array_value)
							SELECT  @Taskset = STUFF(@Taskset, 1, @Separator_position, '')  
						END--while End
						SET @Status=3--updated
				END
				ELSE
				BEGIN
					SET @Status=2--Role Name already Exist
				END
		END
END



SET ANSI_NULLS ON
