IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetProfileIdToSyncES]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetProfileIdToSyncES]
GO

	
-------------------------------------------------
-- Created By : Sadhana Upadhyay on 30 June 2015
-- Summary : To Get profileID list to sync Es index
-------------------------------------------------
CREATE PROCEDURE [dbo].[GetProfileIdToSyncES]
AS
BEGIN
	SELECT ProfileID
		,Action
	FROM ES_LiveListings WITH (NOLOCK)
	WHERE IsSynced = 0
END
