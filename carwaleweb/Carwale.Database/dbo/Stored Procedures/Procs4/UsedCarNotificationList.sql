IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarNotificationList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarNotificationList]
GO

	-- =============================================
-- Author:		<Jitendra>
-- Create date: <03-05-2016>
-- Description:	<This SP used for fetching all used car notification>
-- =============================================
CREATE PROCEDURE [dbo].[UsedCarNotificationList]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT Id,GCMRegId,Content,Url,OSType,NotificationType,UsedCarNotificationId,IMEICode,Title,EntryDate 
	FROM UsedCarAppNotification WITH(NOLOCK)

END
