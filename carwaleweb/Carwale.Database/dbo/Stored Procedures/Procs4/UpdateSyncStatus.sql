IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateSyncStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateSyncStatus]
GO

	
----------------------------------------------------------
-- Created By : Sadhana Upadhyay on 30 June 2015
-- Summary : To update ElasticSearch Sync Status
--Modified By : Sadhana on 3 July 2015 Added Condition for Action name.
----------------------------------------------------------
CREATE PROCEDURE [dbo].[UpdateSyncStatus] 
	 @ProfileId AS VARCHAR(50)
	,@ActionName AS CHAR(6)
AS
BEGIN
	UPDATE ES_LiveListings
	SET IsSynced = 1
		,SyncTime = GETDATE()
	WHERE   ProfileID = @ProfileId
	  AND   [Action]=@ActionName
	  AND IsSynced = 0
END