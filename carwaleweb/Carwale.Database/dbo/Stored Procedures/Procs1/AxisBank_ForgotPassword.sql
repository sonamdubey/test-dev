IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_ForgotPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_ForgotPassword]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 22.12.2013
-- Description:	When a user forgets the password this SP will get update the password salt and hash for the temp passowrd, 
--	password expiry is also set to 1 day for this temp password
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_ForgotPassword] @LoginId VARCHAR(50)
	,@PasswordSalt VARCHAR(10)
	,@PasswordHash VARCHAR(64)
	,@PasswordExpiry DateTime
	,@isUpdated BIT OUTPUT
AS
BEGIN
	SELECT userid
	FROM AxisBank_Users
	WHERE LoginId = @LoginId

	IF @@ROWCOUNT < 1
		SET @isUpdated = 0
	ELSE
	BEGIN
		UPDATE AxisBank_Users
		SET PasswordSalt = @PasswordSalt
			,passwordhash = @passwordhash
			,PasswordExpiry=@PasswordExpiry
			,IsVerified=0
		WHERE LoginId = @LoginId
		Set @isUpdated = 1
	END
END

