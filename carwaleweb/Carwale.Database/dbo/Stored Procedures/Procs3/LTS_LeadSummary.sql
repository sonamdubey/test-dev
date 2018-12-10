IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_LeadSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_LeadSummary]
GO

	
--CREATED ON 09 SEP 2009 BY SENTIL  
--PROCEDURE TO DUMP DAILY ENTRIES OF Lead TRACKED DATA    
    
CREATE PROCEDURE [dbo].[LTS_LeadSummary]    
AS    
BEGIN    
  
  --For Lead Booking data
	INSERT INTO LTS_Sync( CampaignID, CampaignCode, TrackCount, Type, SyncDate )
		Select 
			SourceId AS CampaignId, SourceName AS CampaignCode, COUNT(LeadId) AS Cnt,
			3, CONVERT(VarChar, EntryDate, 101) AS SyncDate
		From CRM_LeadSource
		WHERE
			CONVERT(VarChar, EntryDate, 101) = CONVERT(VarChar, GETDATE() - 1, 101) AND CategoryId = 2
		GROUP By
			SourceId, SourceName, CONVERT(VarChar, EntryDate, 101) 
END    
    
