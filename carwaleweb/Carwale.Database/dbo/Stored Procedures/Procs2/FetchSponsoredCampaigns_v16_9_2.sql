IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchSponsoredCampaigns_v16_9_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchSponsoredCampaigns_v16_9_2]
GO

	-- =============================================      
-- Author: Shalini Nair
-- Create date: 20/08/2015
-- Description: Fetch sponsored campaigns based on platform and CampaignCateRgory or all the sponsoredcampaigns
-- Modifier : Sachin Bharti on 4/4/2016
-- Description : Get default campaign if no featured campaign is running
-- Modified by rohan 7th june 2016, changes for app monetization.
--		added sponsored Links and targeting for specific monetized app screens(model,version screen and compare detail screen)
-- Modified By : Sachin Bharti(20th June 2016)
-- Purpose : Added IsDefault condition for the first select query
-- Modified By: Rakesh Yadav on 02 Sep 2016, fetch next active campaign start date
-- Modified By: Rakesh Yadav on 21 Sep 2016, Removed PayLoad switch case
-- =============================================      
CREATE PROCEDURE [dbo].[FetchSponsoredCampaigns_v16_9_2] @CampaignCategoryId INT = NULL
	,@PlatformId INT = NULL
	,@CategorySection INT = NULL
	,@Param INT = NULL --modelid/versionId
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from      
	-- interfering with SELECT statements.      
	SET NOCOUNT ON;

	DECLARE @nextCampaignStartDate DATETIME

	SET @nextCampaignStartDate = GETDATE() + 1

	SELECT TOP 1 @nextCampaignStartDate = StartDate
	FROM SponsoredCampaigns AS SC WITH (NOLOCK)
	WHERE IsActive = 1
		AND IsDeleted = 0
		AND ISNULL(IsDefault, 0) = 0 -- Sachin Bharti(20th June 2016)
		AND GETDATE() < StartDate
		AND (
			@CampaignCategoryId IS NULL
			OR CampaignCategoryId = @CampaignCategoryId
			)
		AND (
			@PlatformId IS NULL
			OR PlatformId = @PlatformId
			)
		AND (
			@CategorySection IS NULL
			OR CategorySection = @CategorySection
			)
		AND (
			CampaignCategoryId <> 8
			OR (
				CampaignCategoryId = 8
				AND CategorySection NOT IN (
					3
					,4
					,6
					)
				)
			OR (
				CampaignCategoryId = 8
				AND CategorySection IN (
					3
					,4
					,6
					)
				AND @Param IS NOT NULL
				AND Id IN (
					SELECT CampaignId
					FROM NativeAppAdsTargeting WITH (NOLOCK)
					WHERE (
							TargetModelId = @Param
							OR TargetVersionId = @Param
							)
						AND IsActive = 1
					)
				)
			)
	ORDER BY SC.StartDate ASC;

	WITH CTE
	AS (
		SELECT SC.Id
			,SC.ModelId
			,SC.AdScript
			,SC.IsActive
			,SC.CampaignCategoryId
			,SC.StartDate
			,SC.EndDate
			,SC.IsSponsored
			,SC.IsDeleted
			,SC.IsDefault
			,SC.PlatformId
			,SC.CreatedBy
			,SC.CreatedOn
			,SC.UpdatedOn
			,SC.UpdatedBy
			,SC.VPosition
			,SC.HPosition
			,SC.ImageUrl
			,SC.JumbotronPos
			,SC.BackGroundColor
			,SC.CategorySection
			,SC.MakeId
			,SC.SponsoredTitle
			,SC.Subtitle
			,SC.CardHeader
			,SC.Position
			--
			,1 AS RowOrder
		FROM SponsoredCampaigns AS SC WITH (NOLOCK)
		WHERE IsActive = 1
			AND IsDeleted = 0
			AND ISNULL(IsDefault, 0) = 0 -- Sachin Bharti(20th June 2016)
			AND GETDATE() BETWEEN StartDate
				AND EndDate
			AND (
				@CampaignCategoryId IS NULL
				OR CampaignCategoryId = @CampaignCategoryId
				)
			AND (
				@PlatformId IS NULL
				OR PlatformId = @PlatformId
				)
			AND (
				@CategorySection IS NULL
				OR CategorySection = @CategorySection
				OR (@CampaignCategoryId =6 AND @PlatformId = 74 AND @CategorySection = 1)
				)
			AND (
				CampaignCategoryId <> 8
				OR (
					CampaignCategoryId = 8
					AND CategorySection NOT IN (
						3
						,4
						,6
						)
					)
				OR (
					CampaignCategoryId = 8
					AND CategorySection IN (
						3
						,4
						,6
						)
					AND @Param IS NOT NULL
					AND Id IN (
						SELECT CampaignId
						FROM NativeAppAdsTargeting WITH (NOLOCK)
						WHERE (
								TargetModelId = @Param
								OR TargetVersionId = @Param
								)
							AND IsActive = 1
						)
					)
				)
		
		UNION
		
		SELECT SC.Id --
			,SC.ModelId --
			,SC.AdScript --
			,SC.IsActive --
			,SC.CampaignCategoryId --
			,SC.StartDate --
			,SC.EndDate --
			,SC.IsSponsored --
			,SC.IsDeleted --
			,SC.IsDefault --
			,SC.PlatformId --
			,SC.CreatedBy
			,SC.CreatedOn
			,SC.UpdatedOn
			,SC.UpdatedBy
			,SC.VPosition
			,SC.HPosition
			,SC.ImageUrl
			,SC.JumbotronPos
			,SC.BackGroundColor
			,SC.CategorySection
			--
			,SC.MakeId
			,SC.SponsoredTitle
			,SC.Subtitle
			,SC.CardHeader
			,SC.Position
			--
			,0 AS RowOrder
		FROM SponsoredCampaigns AS SC WITH (NOLOCK)
		WHERE IsActive = 1
			AND IsDeleted = 0
			AND IsDefault = 1
			AND (
				@CampaignCategoryId IS NULL
				OR CampaignCategoryId = @CampaignCategoryId
				)
			AND (
				@PlatformId IS NULL
				OR PlatformId = @PlatformId
				)
			AND (
				@CategorySection IS NULL
				OR CategorySection = @CategorySection
				)
		)
	SELECT TOP 1 Id
		,ModelId
		,AdScript
		,IsActive
		,CampaignCategoryId
		,StartDate
		,EndDate
		,IsSponsored
		,IsDeleted
		,IsDefault
		,PlatformId
		,CreatedBy
		,CreatedOn
		,UpdatedOn
		,UpdatedBy
		,VPosition
		,HPosition
		,ImageUrl
		,JumbotronPos
		,BackGroundColor
		,CategorySection
		,MakeId
		,SponsoredTitle
		,Subtitle
		,CardHeader
		,Position
	INTO #temp_copy
	FROM CTE
	ORDER BY RowOrder DESC

	SELECT *
	FROM #temp_copy

	SELECT SL.Id
		,SL.CampaignId
		,SL.NAME
		,SL.IsInsideApp
		,SL.Url
		,SL.IsUpcoming
		,'modelId=' + CONVERT(VARCHAR, TC.ModelId) AS PayLoad --Removed PayLoad switch case

	FROM SponsoredLinks SL WITH (NOLOCK)
	INNER JOIN #temp_copy TC ON TC.Id = SL.CampaignId

	SELECT @nextCampaignStartDate AS NextCampaignStartDate

	DROP TABLE #temp_copy;
END
