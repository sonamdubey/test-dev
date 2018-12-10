IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LA_CheckLoginData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LA_CheckLoginData]
GO

	CREATE PROCEDURE [dbo].[LA_CheckLoginData]

	@UserName				VarChar(50),
	@Password				VarChar(50),
	@IsTesting				Bit OutPut,
	@LAId					Numeric OutPut	
				
 AS
	DECLARE
		@FetchedId Numeric
BEGIN
	SET @LAId = -1
	
	SELECT @FetchedId = ID, @IsTesting = IsTesting FROM LA_Agencies
	WHERE UserName = @UserName AND Password = @Password AND IsActive = 1

	IF @@ROWCOUNT <> 0
		BEGIN
			SET @LAId = @FetchedId 
		END
	ELSE
		BEGIN
			SET @LAId = -1
		END
END