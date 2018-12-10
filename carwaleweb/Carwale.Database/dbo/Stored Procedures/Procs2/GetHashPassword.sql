IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHashPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHashPassword]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	26th Nov 2013
-- Description	:	Get hash password for Opr_Users 
-- =============================================
CREATE PROCEDURE [dbo].[GetHashPassword]
	@UserId			INT = NULL,
	@LoginId		VARCHAR(25) = NULL,
	@HashPassword	VARCHAR(70) = NULL OUTPUT ,
	@Salt			VARCHAR(10) = NULL OUTPUT ,
	@LoginUserId	INT	= -1 OUTPUT,
	@CLExtension	NUMERIC = -1	OUTPUT ,--THIS IS clextension map of the user
	@DCRMExtension	NUMERIC = NULL OUTPUT --This extension used for DCRMTasklist page
AS
	BEGIN
		IF @UserId IS NOT NULL
		BEGIN
			SELECT @HashPassword = OU.PasswordHash ,@LoginUserId = OU.Id, @Salt = OU.HashSalt
			FROM OprUsers OU(NOLOCK) WHERE OU.Id = @UserId
		END
		ELSE IF @LoginId IS NOT NULL
		BEGIN
			SELECT @HashPassword = OU.PasswordHash,@LoginUserId = OU.Id,@Salt = OU.HashSalt
			FROM OprUsers OU(NOLOCK) WHERE OU.LoginId = @LoginId
		END

		IF @LoginUserId <> -1
		BEGIN
			SELECT @CLExtension = Extension FROM CL_ExtensionMap WHERE UserId = @LoginUserId
			SELECT @DCRMExtension = Id FROM DCRM_UserExtensions WHERE UserId = @LoginUserId
		END
	END
