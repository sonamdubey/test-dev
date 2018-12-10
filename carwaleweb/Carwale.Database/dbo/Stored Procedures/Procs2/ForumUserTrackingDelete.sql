IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ForumUserTrackingDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ForumUserTrackingDelete]
GO

	
CREATE PROCEDURE [dbo].[ForumUserTrackingDelete]
AS
BEGIN

	DELETE FROM ForumUserTracking 
	WHERE ActivityDateTime < CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),103),103)

END
