IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_InsertSourceTracking]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_InsertSourceTracking]
GO

	--Modified by:  Manish on 07-10-2014 added with (nolock) keyword in the select statement
--Modified by:  Avishkar on 15-12-2015 commented
--Procedure is for Source Tracking    
 ---Modified by Manish on 17-12-2015 passed SCOPE IDENTITY
CREATE PROCEDURE [dbo].[LTS_InsertSourceTracking]      
(      
 @CampaignCode AS VARCHAR(100),    
 @EntryDateTime AS DATETIME,    
 @IPAddress AS VARCHAR(50),    
 @PreviousSTId AS BIGINT,  
 @LandingURL AS VARCHAR(100),	
 @SOURCEID AS BIGINT OUT,
 @CampaignId AS NUMERIC OUT  
)      
AS      
BEGIN      
       
SELECT @CampaignID = ID  FROM LTS_Campaigns WITH (NOLOCK) WHERE CampaignCode = @CampaignCode  
  
IF @@ROWCOUNT = 0  
BEGIN  
 SET @CampaignID = '-1'  
END  
  
INSERT INTO       
 LTS_SourceTracking ( CampaignId, CampaignCode, EntryDateTime, IPAddress, PreviousSTId, LandingURL)     
     VALUES(@CampaignId,@CampaignCode,@EntryDateTime,@IPAddress,@PreviousSTId,@LandingURL)      
    

        SET @SOURCEID =  SCOPE_IDENTITY()             ---Modified by Manish on 17-12-2015 passed SCOPE IDENTITY 

--Modified by:  Avishkar on 15-12-2015 commented
 
--SELECT @SOURCEID = ID FROM LTS_SourceTracking WITH (NOLOCK) WHERE CampaignCode = @CampaignCode  
--IF @@ROWCOUNT = 0  
--BEGIN  
  
--END  
      
END 