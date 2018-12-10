IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FCMTokenValidation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FCMTokenValidation]
GO

	-- =============================================
-- Author:		Meet Shah
-- Create date: 2/11/2016
-- Description:	FCM token validation service SP
-- =============================================
CREATE  PROCEDURE [dbo].[FCMTokenValidation] 
	-- Add the parameters for the stored procedure here
	@MobileUserId INT, 
	@ToBeDeleted BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert statements for procedure here
	IF @ToBeDeleted = 1
	BEGIN
		INSERT INTO cwarchive.dbo.MobileUsersLog (MobileUserId,IMEICode,Name,EMailId,ContactNo,OSType,GCMRegId,CreatedOn,LastUpdatedOn,FCMTokenId,UserActiveLastCheckedDate)
		SELECT MobileUserId,IMEICode,Name,EMailId,ContactNo,OSType,GCMRegId,CreatedOn,LastUpdatedOn,FCMTokenId,UserActiveLastCheckedDate 
		FROM Mobile.MobileUsers WITH (NOLOCK) 
		WHERE MobileUserId = @MobileUserId
		
		INSERT INTO cwarchive.dbo.UserSubscriptionMappingLog (Id,MobileUserId,SubsMasterId,IsActive,CreatedOn,LastUpdatedOn)
		SELECT Id,MobileUserId,SubsMasterId,IsActive,CreatedOn,LastUpdatedOn 
		FROM  Mobile.UserSubscriptionMapping WITH (NOLOCK) 
		WHERE MobileUserId = @MobileUserId
		
		DELETE FROM Mobile.MobileUsers 
		WHERE MobileUserId = @MobileUserId
		
		DELETE FROM Mobile.UserSubscriptionMapping 
		WHERE MobileUserId = @MobileUserId
	END
    ELSE
	BEGIN
		UPDATE Mobile.MobileUsers SET UserActiveLastCheckedDate = GETDATE() WHERE MobileUserId = @MobileUserId
	END
END
