IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarDetailViewlogDeleteData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarDetailViewlogDeleteData]
GO

	
CREATE PROCEDURE [dbo].[UsedCarDetailViewlogDeleteData] 	
AS
BEGIN
	DECLARE @LogDeletionSpan SMALLINT;
	DECLARE @CurrentDate datetime= GETDATE();
	SELECT @LogDeletionSpan = Span 
	FROM UsedCarNotificationConfig WITH(NOLOCK) 
	WHERE Id=5;

	DELETE from Usedcardetailviewlog
	WHERE EntryTime < @CurrentDate -@LogDeletionSpan;
END