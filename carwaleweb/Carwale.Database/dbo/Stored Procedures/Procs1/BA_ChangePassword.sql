IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_ChangePassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_ChangePassword]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 28-may-14
-- Description:	Update the password for the User.
-- =============================================
CREATE PROCEDURE [dbo].[BA_ChangePassword]
@Password VARCHAR(50),
@UserName VARCHAR(50),
@NewPassword VARCHAR(50)

AS
BEGIN
DECLARE @Status TINYINT = 0 ---Unsuccessfull
	SET NOCOUNT ON;

	-- Validate
	IF (SELECT Password FROM BA_Login WITH (NOLOCK) WHERE UserName = @UserName) = @Password
	BEGIN
	UPDATE [dbo].[BA_Login] 
			SET  [Password] = @NewPassword WHERE UserName = @UserName 
   SET @Status = @@ROWCOUNT --Get the no. of rows  affected by the last statement
   END

   SELECT @Status

END
