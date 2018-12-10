IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveUserPwdRecoveryToken]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveUserPwdRecoveryToken]
GO

	-- ===================================================================================
-- Author		:	Suresh Prajapati
-- Create date	:	08th Mar, 2016
-- Description	:	Procedure to save or update the password recovery tokens for TC_Users
-- ===================================================================================
CREATE PROCEDURE [dbo].[TC_SaveUserPwdRecoveryToken]
	-- Add the parameters for the stored procedure here
	@TC_UserId INT
	,@Token VARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IsNew BIT = 1

	IF EXISTS (
			SELECT TC_UserPasswordRecoveryTokensId
			FROM TC_UserPasswordRecoveryTokens WITH (NOLOCK)
			WHERE TC_UserId = @TC_UserId
			)
		SET @IsNew = 0

	IF @IsNew = 1
	BEGIN
		INSERT INTO TC_UserPasswordRecoveryTokens (
			TC_UserId
			,Token
			,IsActiveToken
			,EntryDate
			,ExpiryDate
			)
		VALUES (
			@TC_UserId
			,@Token
			,1
			,GETDATE()
			,DATEADD("hh", 24, GETDATE())
			)
	END
	ELSE
	BEGIN
		UPDATE TC_UserPasswordRecoveryTokens
		SET Token = @Token
			,IsActiveToken = 1
			,LastUpdated = GETDATE()
			,ExpiryDate = DATEADD("hh", 24, GETDATE())
		WHERE TC_UserId = @TC_UserId
	END
END

