IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ChangePasswordOnAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ChangePasswordOnAlert]
GO

	-- =================================================================================
-- Created by:	Surendra
-- Created Date: 23-12-2011
-- Description:	Alert to TC user to change password if first time login
-- Modified By : Umesh On 9-july-2013 for adding password recovery email
-- Modified By : Suresh Prajapati on 29th Feb, 2016
-- Description : Added HashSalt and PasswordHash changes and removed Password check
-- =================================================================================
CREATE PROCEDURE [dbo].[TC_ChangePasswordOnAlert] @Email VARCHAR(100)
	--,@OldPwd VARCHAR(20)
	--,@NewPwd VARCHAR(20)
	,@PwdRecoveryEmail VARCHAR(50)
	,@IsSpecial TINYINT
	,@HashSalt VARCHAR(10) = NULL
	,@NewPasswordHash VARCHAR(100) = NULL
AS
BEGIN
	DECLARE @ID NUMERIC

	IF (@IsSpecial = 0)
	BEGIN
		SELECT @ID = ID
		FROM TC_Users WITH (NOLOCK)
		WHERE Email = @Email
			--AND [Password] = @OldPwd
			AND IsFirstTimeLoggedIn = 1

		IF (@@ROWCOUNT > 0)
		BEGIN
			UPDATE TC_Users
			SET
				--[Password] = @NewPwd,
				IsFirstTimeLoggedIn = 0
				,PwdRecoveryEmail = @PwdRecoveryEmail
				,HashSalt = @HashSalt
				,PasswordHash = @NewPasswordHash
			WHERE Email = @Email
				--AND [Password] = @OldPwd 
				AND IsActive = 1

			RETURN 0
		END
		ELSE
		BEGIN
			RETURN - 1
		END
	END
	ELSE
	BEGIN
		SELECT @ID = TC_SpecialUsersId
		FROM TC_SpecialUsers WITH (NOLOCK)
		WHERE Email = @Email
			--AND [Password] = @OldPwd
			AND IsFirstTimeLoggedIn = 1

		IF (@@ROWCOUNT > 0)
		BEGIN
			UPDATE TC_SpecialUsers
			SET
				--[Password] = @NewPwd,
				IsFirstTimeLoggedIn = 0
				,PwdRecoveryEmail = @PwdRecoveryEmail
			WHERE Email = @Email
				--AND [Password] = @OldPwd
				AND IsActive = 1

			RETURN 0
		END
		ELSE
		BEGIN
			RETURN - 1
		END
	END
END

