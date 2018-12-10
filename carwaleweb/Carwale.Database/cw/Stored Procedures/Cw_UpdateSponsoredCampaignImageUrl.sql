IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Cw_UpdateSponsoredCampaignImageUrl]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Cw_UpdateSponsoredCampaignImageUrl]
GO

	
--*******************************************************************

--Created By:Supreksha Singh on 08-11-2016      
--Description:To update ImageUrl               
CREATE PROCEDURE [cw].[Cw_UpdateSponsoredCampaignImageUrl]
	@CampaignId NUMERIC,                	
	@ImageUrl VARCHAR(300)= NULL 
AS                
BEGIN                
SET NOCOUNT ON;       
      
	DECLARE
	@IsUpdated	BIT = NULL

	UPDATE SponsoredCampaigns 
		SET                					
			ImageUrl = @ImageUrl
		WHERE 
			Id = @CampaignId 
		IF @@ROWCOUNT > 0
			SET @IsUpdated = 1		    
		select @IsUpdated as IsUpdated       
END              					   	

