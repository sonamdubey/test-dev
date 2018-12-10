IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchSponsoredCampaigns_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchSponsoredCampaigns_v16]
GO

	-- =============================================      
-- Author: Shalini Nair
-- Create date: 20/08/2015
-- Description: Fetch sponsored campaigns based on platform and CampaignCategory or all the sponsoredcampaigns
-- Modifier : Sachin Bharti on 4/4/2016
-- Description : Get default campaign if no featured campaign is running
-- Modified By : Sachin Bharti(20th June 2016)
-- Purpose : Added IsDefault condition for the first select query
-- =============================================      
CREATE PROCEDURE [dbo].[FetchSponsoredCampaigns_v16.4.1]    
@CampaignCategoryId int = null,
@PlatformId int=null,
@CategorySection int=null  
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
	WITH CTE AS (
	SELECT	SC.Id,SC.ModelId,SC.AdScript,SC.IsActive,SC.CampaignCategoryId,SC.StartDate,SC.EndDate,
			SC.IsSponsored,SC.IsDeleted,SC.IsDefault,SC.PlatformId,SC.CreatedBy,SC.CreatedOn,SC.UpdatedOn,SC.UpdatedBy,
			SC.VPosition,SC.HPosition,SC.ImageUrl,SC.JumbotronPos,SC.BackGroundColor,SC.CategorySection,
			1 AS RowOrder 
	FROM SponsoredCampaigns AS SC WITH (NOLOCK)      
	WHERE IsActive=1 AND IsDeleted=0 AND IsNULL(IsDefault,0) =  0
		AND GETDATE() BETWEEN StartDate AND EndDate
		AND (@CampaignCategoryId IS NULL OR CampaignCategoryId = @CampaignCategoryId)
		AND (@PlatformId is null or PlatformId = @PlatformId)
		AND (@CategorySection is null OR CategorySection = @CategorySection)

	UNION 

	SELECT	SC.Id,SC.ModelId,SC.AdScript,SC.IsActive,SC.CampaignCategoryId,SC.StartDate,SC.EndDate,
			SC.IsSponsored,SC.IsDeleted,SC.IsDefault,SC.PlatformId,SC.CreatedBy,SC.CreatedOn,SC.UpdatedOn,SC.UpdatedBy,
			SC.VPosition,SC.HPosition,SC.ImageUrl,SC.JumbotronPos,SC.BackGroundColor,SC.CategorySection,
			0 AS RowOrder 
	FROM SponsoredCampaigns AS SC WITH (NOLOCK)      
	WHERE IsActive=1 AND IsDeleted=0 AND IsDefault = 1
		AND (@CampaignCategoryId IS NULL OR CampaignCategoryId = @CampaignCategoryId)
		AND (@PlatformId IS NULL OR PlatformId = @PlatformId)
		AND (@CategorySection is null OR CategorySection = @CategorySection))
		
	SELECT TOP 1 * FROM CTE 
    ORDER BY RowOrder DESC
END 