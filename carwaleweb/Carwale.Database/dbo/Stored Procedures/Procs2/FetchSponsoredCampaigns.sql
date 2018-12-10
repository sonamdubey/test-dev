IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchSponsoredCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchSponsoredCampaigns]
GO

	-- =============================================      
-- Author: Shalini Nair
-- Create date: 20/08/2015
-- Description: Fetch sponsored campaigns based on platform and CampaignCategory or all the sponsoredcampaigns
-- =============================================      
CREATE PROCEDURE [dbo].[FetchSponsoredCampaigns]    
@CampaignCategoryId int = null,
@PlatformId int=null  
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
SELECT * FROM SponsoredCampaigns WITH (NOLOCK)      
WHERE IsActive=1 AND IsDeleted=0 AND GETDATE() BETWEEN StartDate AND EndDate 
and (@CampaignCategoryId is null or CampaignCategoryId = @CampaignCategoryId) and (@PlatformId is null or PlatformId = @PlatformId)
order by StartDate desc  
     
END 
