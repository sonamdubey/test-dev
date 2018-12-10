IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_STSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_STSummary]
GO

	--Procedure To Dump Daily Entries of Source tracked data
CREATE PROCEDURE [dbo].[LTS_STSummary]  
AS  
BEGIN  
	Insert Into LTS_Sync ( CampaignID, CampaignCode, SyncDate, TrackCount, TYPE)
	Select 
		CampaignId, CampaignCode, CONVERT(VarChar, EntryDateTime, 101) AS SyncDate, 
		COUNT(Id) AS TrackCount, 0 AS TYPE
	From LTS_SourceTracking
	Where	
		CONVERT(VarChar, EntryDateTime, 101) = CONVERT(VarChar, GETDATE() - 1, 101)
	Group By 
		CampaignId, CampaignCode, CONVERT(VarChar, EntryDateTime, 101)
END  
