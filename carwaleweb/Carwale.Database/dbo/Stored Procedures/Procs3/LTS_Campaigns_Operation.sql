IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_Campaigns_Operation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_Campaigns_Operation]
GO

	CREATE PROCEDURE [dbo].[LTS_Campaigns_Operation]  
(    
 @ID AS BIGINT = 0,
 @SourceId AS BIGINT = 0,
 @CampaignCode AS VARCHAR(100) = NULL,  
 @CampaignName AS VARCHAR(100) = NULL, 
 @CampaignDesc AS VARCHAR(500) = NULL,
 @IsActive AS BIT,     
 @StartDate AS DATETIME,  
 @opr AS BIGINT = 0  
)    
AS    
BEGIN    
     
	IF(@opr = 1 )  
		 BEGIN  
			  INSERT INTO   
				  LTS_Campaigns ( SourceId, CampaignCode, CampaignName, IsActive, CampaignDesc, StartDate)    
				  VALUES(@SourceId,@CampaignCode,@CampaignName, @IsActive, @CampaignDesc,@StartDate)    
		 END  
	ELSE IF(@opr = 2 )  
		 BEGIN  
			  UPDATE LTS_Campaigns   
			  SET  CampaignName = @CampaignName, IsActive = @IsActive, CampaignDesc = @CampaignDesc  WHERE ID = @ID  
		 END  
END  
