IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WebNotificationSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WebNotificationSave]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <15 th dec 15>
-- Description:	<Save web notifications if new lead is added or lead transfered>
-- =============================================
CREATE PROCEDURE [dbo].[TC_WebNotificationSave]
	@TC_UserId INT,
	@RecordId	INT,
	@RecordType SMALLINT
AS
BEGIN

	INSERT INTO TC_Notifications(TC_UserId,RecordId,RecordType,NotificationDateTime)
	VALUES(@TC_UserId,@RecordId,@RecordType,GETDATE())

	IF EXISTS(SELECT Id FROM TC_Users WITH(NOLOCK) WHERE Id = @TC_UserId AND GCMRegistrationId IS NOT NULL)
		BEGIN
			INSERT INTO TC_AppNotification(TC_UserId,RecordId,RecordType,NotificationDateTime)
			VALUES(@TC_UserId,@RecordId,@RecordType,GETDATE())
		END	
END