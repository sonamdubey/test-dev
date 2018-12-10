IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_ResetPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_ResetPassword]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_ResetPassword]
@UserName VARCHAR(50),
@NewPassword VARCHAR(50)

AS
BEGIN
DECLARE @Status TINYINT = 0 ---Unsuccessfull
	SET NOCOUNT ON;

	UPDATE [dbo].[BA_Login] 
   SET  [Password] = @NewPassword WHERE 
								UserName = @UserName 
   SET @Status = @@ROWCOUNT --Get the no. of rows  affected by the last statement

   SELECT @Status AS Status--Output

END
