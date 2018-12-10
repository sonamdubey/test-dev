IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSponsoredCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSponsoredCampaigns]
GO

	--*****************************************************************************
--Created By Rakesh Yadav on 1 Sep 2016, to fetch all Campaigns on selected filters
--Modified By: Rakesh yadav on 2 Nov 2016, add isDeleted filter
CREATE PROCEDURE [dbo].[GetSponsoredCampaigns]
@CampaignCategoryId INT= null,
@PlatformId INT = NULL,
@SectionId INT = NULL,
@MakeId INT = NULL,
@ModelId INT = NULL,
@Type SMALLINT,
@IncludeUpcoming BIT=1
AS 

BEGIN

	SELECT distinct SC.Id AS CampaignId, SC.ModelId,V.Make+' '+V.Model AS CarName,SC.CampaignCategoryId,CC.Name AS CampaignCategory,
	SC.CategorySection AS CategorySectionId, CS.Name AS CategorySection,SC.PlatformId,SC.CardHeader,SC.SponsoredTitle 
	,SC.Subtitle,SC.AdScript,SC.StartDate,SC.EndDate,
	SC.VPosition,SC.HPosition,SC.ImageUrl,SC.JumbotronPos,SC.IsSponsored, SC.IsActive,sc.IsDefault as IsDefaultCampaign
	from SponsoredCampaigns SC WITH(NOLOCK)
	JOIN CampaignCategory CC WITH(NOLOCK) ON CC.Id=SC.CampaignCategoryId
	LEFT JOIN CategorySection CS WITH(NOLOCK) ON CS.Id=SC.CategorySection
	LEFT JOIN vwMMV V WITH(NOLOCK) ON SC.ModelId=V.ModelId
	WHERE 
	(@CampaignCategoryId IS NULL OR SC.CampaignCategoryId=@CampaignCategoryId)
	AND (@PlatformId IS NULL OR SC.PlatformId=@PlatformId)
	AND (@SectionId IS NULL OR SC.CategorySection=@SectionId)
	AND (@MakeId IS NULL OR V.MakeId=@MakeId)
	AND (@ModelId IS NULL OR SC.ModelId=@ModelId)
	AND
	(
	(@Type=1 AND GETDATE() BETWEEN SC.StartDate AND SC.EndDate AND SC.IsActive=1)
	OR
	(@Type=2 AND (GETDATE()> SC.EndDate OR SC.IsActive=0))
	OR
	(@Type=3 AND GETDATE() < StartDate )
	)
	AND IsDeleted=0
END

