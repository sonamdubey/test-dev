IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Changepassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Changepassword]
GO

	
-- =========================================================================
--Modified by Umesh on 18-07-2013 for password recovery functionality
-- Modifeid By : Suresh Prajapati on 11th Marc, 2016
-- Description : Updated HashSalt and PasswordHash instead of Password
-- =========================================================================
CREATE PROCEDURE [dbo].[TC_Changepassword] @UserID NUMERIC
	--,@OldPwd VARCHAR(20)
	--,@NewPwd VARCHAR(20)
	,@IsSpecial TINYINT
	,@UpdateStatus NUMERIC OUTPUT
	,@HashSalt VARCHAR(10)= NULL
	,@PasswordHash VARCHAR(100)= NULL
AS
BEGIN
	DECLARE @ID NUMERIC

	IF (@IsSpecial = 0)
	BEGIN
		SELECT @ID = ID
		FROM TC_Users WITH(NOLOCK)
		WHERE ID = @UserID
			--AND [Password] = @OldPwd

		IF (@@ROWCOUNT > 0)
		BEGIN
			UPDATE TC_Users
			SET HashSalt=@HashSalt
			,PasswordHash=@PasswordHash
			WHERE ID = @UserID
				--AND [Password] = @OldPwd

			SET @UpdateStatus = 1
		END
		ELSE
		BEGIN
			SET @UpdateStatus = - 1
		END
	END
	ELSE
	BEGIN
		SELECT @ID = TC_SpecialUsersId
		FROM TC_SpecialUsers WITH(NOLOCK)
		WHERE TC_SpecialUsersId = @UserID
			--AND [Password] = @OldPwd

		--IF (@@ROWCOUNT > 0)
		--BEGIN
		--	UPDATE TC_SpecialUsers
		--	SET [Password] = @NewPwd
		--	WHERE TC_SpecialUsersId = @UserID
		--		AND [Password] = @OldPwd

		--	SET @UpdateStatus = 1
		--END
		--ELSE
		--BEGIN
		--	SET @UpdateStatus = - 1
		--END
	END
END

