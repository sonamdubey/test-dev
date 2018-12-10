IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_Users_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_Users_SP]
GO

	




-- =============================================
-- Author:		Umesh
-- Create date: 13-10-2011
-- Description:	User details of under different branches.
-- =============================================
CREATE PROCEDURE [dbo].[NCD_Users_SP]
	-- Add the parameters for the stored procedure here
	@Id INT = NULL,
	@DealerId INT,
	@RoleId INT = NULL,
	@UserName VARCHAR(50), 
	@Email VARCHAR(100),
	@Mobile VARCHAR(10),
	@Password VARCHAR(20),
	@DOB DATE=NULL,
	@DOJ DATE =NULL,
	@Sex VARCHAR(6),
	@Address VARCHAR(200),
	@Status INT OUTPUT
	
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
			IF NOT EXISTS(SELECT top 1 Id FROM NCD_Users WHERE Email = @Email)
				BEGIN
					INSERT INTO NCD_Users(DealerId,RoleId, UserName, Email, Password, Mobile, EntryDate, DOB, DOJ, Sex, Address)
					VALUES(@DealerId, @RoleId, @UserName, @Email, @Password, @Mobile, GETDATE(), @DOB, @DOJ,  @Sex, @Address)
					set @Status=1
				END				
		END
	Else --IF id contaiing data, we updatig user information
		BEGIN
			UPDATE NCD_Users set DealerId=@DealerId, RoleId=@RoleId, UserName=@UserName, Email=@Email, Password=@Password, Mobile=@Mobile, DOB=@DOB, DOJ=@DOJ, Sex=@Sex, Address=@Address WHERE Id = @Id
			set @Status=2
		END	
END





