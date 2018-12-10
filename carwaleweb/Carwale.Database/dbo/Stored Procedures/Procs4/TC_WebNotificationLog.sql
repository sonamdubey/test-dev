IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WebNotificationLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WebNotificationLog]
GO

	
--------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <15th Dec 15>
-- Description:	<Log read notifications>
--TC_WebNotificationLog 1,243,2
-- =============================================
CREATE PROCEDURE [dbo].[TC_WebNotificationLog]
	@TC_NotificationsId VARCHAR(MAX),
	@TC_UserId INT,
	@NotificationType TINYINT  -- 1 for web 2 for android app
AS
BEGIN

	IF @NotificationType = 1
		BEGIN
			-- LOG READ DATA IN TC_NOTIFICATIONSLOG
			INSERT INTO TC_NotificationsLog (TC_UserId,RecordId,RecordType,NotificationDateTime,ReadDateTime)
			SELECT TC_UserId,RecordId,RecordType,NotificationDateTime,GETDATE()
			FROM TC_Notifications TN WITH(NOLOCK)
			WHERE TC_NotificationsId IN (SELECT ListMember FROM fnSplitCSV(@TC_NotificationsId))
			AND TN.TC_UserId=@TC_UserId

			-- DELETE READ DATA FROM TC_NOTIFICATIONS
			DELETE FROM TC_Notifications WHERE TC_NotificationsId IN (SELECT ListMember FROM fnSplitCSV(@TC_NotificationsId))
			AND TC_UserId=@TC_UserId
		END
	ELSE IF @NotificationType = 2
		BEGIN
			-- LOG READ DATA IN TC_NOTIFICATIONSLOG
			INSERT INTO TC_AppNotificationsLog (TC_UserId,RecordId,RecordType,NotificationDateTime,SentDateTime)
			SELECT TC_UserId,RecordId,RecordType,NotificationDateTime,GETDATE()
			FROM TC_AppNotification TN WITH(NOLOCK)
			WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@TC_NotificationsId))

			-- DELETE READ DATA FROM TC_NOTIFICATIONS
			DELETE FROM TC_AppNotification WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@TC_NotificationsId))
		END


	
END

