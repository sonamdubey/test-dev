IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_CTSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_CTSummary]
GO

	--CREATED ON 09 SEP 2009 BY SENTIL
--PROCEDURE TO DUMP DAILY ENTRIES OF CONVERSION TRACKED DATA  
-- AM Modified 6-7-2012 
  
CREATE PROCEDURE [dbo].[LTS_CTSummary]  
AS  
BEGIN 

    DECLARE  @PrevDate date
    SET @PrevDate=  GETDATE() 
	Insert Into LTS_Sync ( CampaignID, CampaignCode, SyncDate, TrackCount, TYPE)
	Select 
		LST.CampaignId, LST.CampaignCode, CONVERT(VarChar, LCT.EntryDateTime, 101) AS SyncDate, 
		COUNT(Distinct LCT.StdId) AS TrackCount, 1 AS TYPE
	From LTS_SourceTracking AS LST WITH(NOLOCK), 
	     LTS_ConversionTracking AS LCT WITH(NOLOCK)
	Where
		LST.ID = LCT.StdId AND
		--LCT.EntryDateTime<@PrevDate
		-- AM Modified 6-7-2012
		CONVERT(VarChar, LCT.EntryDateTime, 101) = CONVERT(VarChar, GETDATE() - 1, 101)
	Group By 
		LST.CampaignId, LST.CampaignCode, CONVERT(VarChar, LCT.EntryDateTime, 101)

  
END  
  
  