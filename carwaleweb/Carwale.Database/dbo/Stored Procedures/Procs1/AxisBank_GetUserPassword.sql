IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetUserPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetUserPassword]
GO

	-- =============================================
-- Author:	Akansha
-- Create date: 12.11.2013
-- Description:	Proc to get salt and hash of the User
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetUserPassword]
	-- Add the parameters for the stored procedure here
	@UserId BIGINT ,
	@Email VARCHAR(100) OUTPUT,
	@Salt VARCHAR(10) OUTPUT,
	@Hash VARCHAR(64) OUTPUT,
	@FirstName VARCHAR(50) OUTPUT,
	@LastName VARCHAR(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT @Salt = PasswordSalt, @Hash = PasswordHash, @Email = Email, @FirstName = FirstName,@LastName=LastName FROM AxisBank_Users WHERE UserId = @UserId
END
