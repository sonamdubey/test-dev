IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_UpdateUserPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_UpdateUserPassword]
GO

	-- =============================================
-- Author:	Akansha
-- Create date: 12.11.2013
-- Description:	Proc to update salt and hash of the User
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_UpdateUserPassword]
	-- Add the parameters for the stored procedure here
	@UserId BIGINT,
	@Salt VARCHAR(10),
	@Hash VARCHAR(64),
	@PasswordExpiry datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE AxisBank_Users
		SET PasswordSalt = @Salt, PasswordHash = @hash, IsVerified=1, PasswordExpiry=@PasswordExpiry
	WHERE UserId = @UserId 

	exec AxisBank_InsertInPasswordLog @UserId,@Salt,@Hash
END