IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdatePwdRecoveryTokenStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdatePwdRecoveryTokenStatus]
GO

	-- ================================================================================
-- Author		: Suresh Prajapti
-- Created On	: 08th Mar, 2016
-- Description	: This procedure will inactive active token for a specified user id
-- ================================================================================
CREATE PROCEDURE [dbo].[TC_UpdatePwdRecoveryTokenStatus]
	-- Add the parameters for the stored procedure here
	@TC_UserId VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE TC_UserPasswordRecoveryTokens
	SET IsActiveToken = 0
		,LastUpdated = GETDATE()
	WHERE TC_UserId = @TC_UserId
END


