IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Cw_GetCampaignData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Cw_GetCampaignData]
GO

	-- =============================================
-- Author	:	Sachin Bharti(1st Feb 2016)
-- Description	:	Get campaign data based on campaign id
-- execute Cw_GetCampaignData null,188
-- Modified by : Sachin Bharti(17th March 2016)
-- Description : Get category section for carwale text box search 
-- Modified By : Rakesh Yadav on 23 Aug 2016, to fetch SubTitle,Button & card links,type of button & card links,card header,button text
-- =============================================
CREATE PROCEDURE [dbo].[Cw_GetCampaignData]
	@CategoryId SMALLINT = NULL,
	@CampaignId	INT = NULL,
	@IsActive BIT = NULL
AS
BEGIN
	SELECT 
		DISTINCT(SC.Id),
		SC.AdScript,
		SC.IsActive,
		ISNULL(SC.IsDefault,0) AS IsDefault,
		CASE ISNULL(SC.IsDefault,0) WHEN 1 THEN 'hide' ELSE '' END AS IsDefaultCampaign,
		SC.CampaignCategoryId,
		SC.StartDate,
		SC.EndDate,
		SC.SponsoredTitle,
		SC.Subtitle,
		SC.CardHeader,
		SC.IsSponsored, 
		SC.PlatformId,
		SC.VPosition,
		SC.HPosition,
		SC.ImageUrl,
		SC.JumbotronPos,
		SC.CreatedOn,
		SC.POSITION AS CardPositions,
		CC.Name AS CampaignCategoryName,
		SC.BackGroundColor,
		ISNULL(CS.Id,0) AS CategorySectionId,
		ISNULL(CS.Name,'--') AS CategorySection,
		CM.ID AS CarModelId,
		CM.CarMakeId

	FROM  
		SponsoredCampaigns SC WITH (NOLOCK)
		INNER JOIN CampaignCategory CC WITH (NOLOCK) ON SC.CampaignCategoryId = CC.Id
		LEFT JOIN CategorySection CS(NOLOCK) ON CS.Id = SC.CategorySection
		LEFT JOIN CarModels CM (NOLOCK) ON SC.ModelId = CM.ID
    WHERE 
		(@CampaignId IS NULL OR SC.Id=@CampaignId )
		AND (@CategoryId IS NULL OR SC.CampaignCategoryId = @CategoryId)
		AND (@IsActive IS NULL OR SC.IsActive = @IsActive)
		AND SC.IsDeleted = 0
	ORDER BY
		SC.Id Desc


	select SL.CampaignId,SL.IsInsideApp,SL.Name,SL.Url,SL.UrlOrder,SL.Id AS SponsoredLinkId 
	from SponsoredLinks  SL WITH(NOLOCK)
	JOIN SponsoredCampaigns SC WITH (NOLOCK) ON SL.CampaignId=SC.Id
	where 
	(@CampaignId IS NULL OR SC.Id=@CampaignId )
	AND (@CategoryId IS NULL OR SC.CampaignCategoryId = @CategoryId)
	AND (@IsActive IS NULL OR SC.IsActive = @IsActive)
	AND SC.IsDeleted = 0
	ORDER BY Sc.Id Desc
END

