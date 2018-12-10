IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarAppNotificationLogDeleteData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarAppNotificationLogDeleteData]
GO

	
CREATE PROCEDURE [dbo].[UsedCarAppNotificationLogDeleteData]	
AS
BEGIN
	DECLARE @LogDeletionSpan SMALLINT;
	DECLARE @CurrentDate datetime= GETDATE();
	SELECT @LogDeletionSpan = Span 
	FROM UsedCarNotificationConfig WITH(NOLOCK) 
	WHERE Id=1;

	DELETE from UsedCarAppNotificationLog
	WHERE LastNotified < @CurrentDate -@LogDeletionSpan;
END

