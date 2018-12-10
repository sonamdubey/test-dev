IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UE_SaveNewUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UE_SaveNewUser]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 20-july-2013
-- Description:	Save the user details for unofficial expert 2013
--				If user(email) already exists then return the savedId else newId
-- =============================================
CREATE PROCEDURE [dbo].[OLM_UE_SaveNewUser]
	-- Add the parameters for the stored procedure here
	@FullName		VARCHAR(100),
	@Mobile			VARCHAR(15),
	@Email			VARCHAR(50),
	@DateOfBirth	DATE,
	@Profession		VARCHAR(50),
	@TwitterId		VARCHAR(50),
	@FacebookId		VARCHAR(50),
	@Source         SMALLINT,
	@NewId			NUMERIC OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SET @NewId = -1
    
    --check if that email already exist or not
	SELECT @NewId = Id FROM OLM_UE_Users WHERE Email = @Email
	--If email doesnot exist insert new entry else update details
	IF @@ROWCOUNT = 0
		BEGIN
			--If no email exist then inesert new record for user
			INSERT INTO OLM_UE_Users (FullName, Mobile, Email, DateOfBirth, Profession, TwitterId, FacebookId,Source)
			VALUES (@FullName, @Mobile, @Email, @DateOfBirth, @Profession, @TwitterId, @FacebookId,@Source)
			
			--Set newly generated id to send as output parameter
			SET @NewId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			--If email exist then update details of user and return previousId
			UPDATE OLM_UE_Users
				SET FullName = @FullName, Mobile = @Mobile,
					DateOfBirth = @DateOfBirth, Profession = @Profession,
					TwitterId = @TwitterId, FacebookId = @FacebookId,
					Source = @Source
			WHERE Id = @NewId					
		END
		
END
