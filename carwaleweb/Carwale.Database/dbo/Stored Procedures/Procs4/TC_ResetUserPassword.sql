IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ResetUserPassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ResetUserPassword]
GO

	-- ============================================================
-- Author		: Suresh Prajapti
-- Created On	: 08th Mar, 2016
-- Description	: This procedure will reset the user's password hash and hash salt
-- ============================================================
CREATE PROCEDURE [dbo].[TC_ResetUserPassword]
	-- Add the parameters for the stored procedure here
	@TC_UserId INT
	,@HashSalt VARCHAR(10)
	,@PasswordHash VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE TC_Users
	SET HashSalt = @HashSalt
		,PasswordHash = @PasswordHash
	WHERE Id = @TC_UserId
END


