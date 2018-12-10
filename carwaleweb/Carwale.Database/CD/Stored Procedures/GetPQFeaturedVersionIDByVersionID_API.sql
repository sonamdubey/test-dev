IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetPQFeaturedVersionIDByVersionID_API]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetPQFeaturedVersionIDByVersionID_API]
GO

	-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 18/11/2014
-- Description:	SP to get the featured car
-- modified by sanjay on 9/3/2015 intoduced start date and end date
-- Modified by Manish on 28-09-2015 added with(nolock) keyword wherever not found.
-- =============================================
CREATE PROCEDURE [CD].[GetPQFeaturedVersionIDByVersionID_API] 
@Versions VARCHAR(255),
@CategoryId INT=1,
@PlatformId INT=1,
@CityId INT,
@ZoneId INT=NULL,
@FeaturedVersionId INT=NULL OUTPUT
AS
BEGIN
	IF(@PlatformId = 43) --modified by ashish Verma(for displaying sponsored cars for mobile also same as desktop)
	SET @PlatformId = 1

	DECLARE @tbVersions TABLE (Id INT IDENTITY, VersionId INT)	
	INSERT INTO @tbVersions (VersionId) (select items from [dbo].[SplitTextRS](@Versions,','))
	DECLARE @ComparedVersion INT
	DECLARE @DummyCategoryId INT
	

	SELECT @FeaturedVersionId = FeaturedVersion, @ComparedVersion = VersionId FROM 
	                    (
						Select Top 1 FeaturedVersion, VE.VersionId,PFC.DealerId
						from PQ_FeaturedCampaign PFC WITH(NOLOCK)
						INNER JOIN PQ_FeaturedCampaignRules PFCR WITH(NOLOCK)  ON PFC.Id = PFCR.FeaturedCampaignId
						INNER JOIN @tbVersions VE ON PFCR.TargetVersion = VE.VersionId
						INNER JOIN CarVersions CV WITH(NOLOCK) ON PFCR.FeaturedVersion = CV.ID
						INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
						WHERE FeaturedVersion NOT IN(
							SELECT CV.Id FROM CarVersions CV WITH(NOLOCK)
							WHERE CV.CarModelId IN (
							SELECT DISTINCT CarModelId  FROM CarVersions CV WITH(NOLOCK)
							INNER JOIN @tbVersions VE ON CV.Id = VE.VersionId))
						AND CV.IsDeleted = 0
						AND CV.New = 1
						AND CM.IsDeleted = 0
						AND CM.New = 1
						AND PFC.IsActive = 1
						AND Convert(DATE, PFC.StartDate) <= Convert(DATE, GETDATE()) -- modified by sanjay on 9/3/2015
						AND Convert(DATE, PFC.EndDate) >= Convert(DATE, GETDATE())  -- modified by sanjay on 9/3/2015
						AND	( (PFCR.CityId=@CityId 
									)-- modified by Ashish
								OR PFCR.CityId=-1
							) 				
						ORDER BY VE.Id DESC, PFC.Id DESC) T
END
