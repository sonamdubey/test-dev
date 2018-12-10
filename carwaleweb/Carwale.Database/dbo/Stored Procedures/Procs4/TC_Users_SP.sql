IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Users_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Users_SP]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 13-06-2011
-- Description:	User details of under different branches.
-- Modified By: Tejashree Patil on 5 Sept 2012 on 1 pm
-- Description: Added condition AND IsActive=1 in SELECT clause
-- =============================================
CREATE PROCEDURE [dbo].[TC_Users_SP]
	-- Add the parameters for the stored procedure here
	@Id INT = NULL,
	@BranchId INT,
	@RoleId INT = NULL,
	@UserName VARCHAR(50), 
	@Email VARCHAR(100),
	@Mobile VARCHAR(10),
	@Password VARCHAR(20),
	@DOB DATE=NULL,
	@DOJ DATE =NULL,
	@Sex VARCHAR(6),
	@Address VARCHAR(200),
	@Status INT OUTPUT,
	@ModifiedBy INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
	IF(@Id IS NULL)--IF id parameter is null, we inserting new user to TC_Users table
		BEGIN
			-- Check if user with this email is already registered or not. if not then only register.
			--Modified By: Tejashree Patil on 5 Sept 2012 on 1 pm
			IF NOT EXISTS(SELECT top 1 Id FROM TC_Users WITH(NOLOCK) WHERE Email = @Email AND IsActive=1)
				BEGIN
					INSERT INTO TC_Users(BranchId,RoleId, UserName, Email, Password, Mobile, EntryDate, DOB, DOJ, Sex, Address,ModifiedBy)
					VALUES(@BranchId, @RoleId, @UserName, @Email, @Password, @Mobile, GETDATE(), @DOB, @DOJ,  @Sex, @Address,@ModifiedBy)
					set @Status=1
				END				
		END
		Else --IF id contaiing data, we updatig user information
			BEGIN
				UPDATE TC_Users set BranchId=@BranchId, RoleId=@RoleId, UserName=@UserName, Email=@Email, 
				Password=@Password, Mobile=@Mobile, DOB=@DOB, DOJ=@DOJ, Sex=@Sex, Address=@Address, 
				ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE() WHERE Id = @Id
				set @Status=2
			END	
END





