IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetPQFeaturedVersionIDByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetPQFeaturedVersionIDByVersionID]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 18/11/2014
-- Description:	SP to get the featured car and track the sponsored version 

--modified by ashish Verma on 17-112014 (for displaying sponsored cars for mobile also same as desktop)
-- =============================================
CREATE PROCEDURE [CD].[GetPQFeaturedVersionIDByVersionID] 
@Versions VARCHAR(255),
@CategoryId INT=1,
@PlatformId INT=1,
@CityId INT,
@ZoneId INT,
@FeaturedVersionId INT=NULL OUTPUT
AS
BEGIN
	IF(@PlatformId = 43) --modified by ashish Verma(for displaying sponsored cars for mobile also same as desktop)
	SET @PlatformId = 1

	DECLARE @tbVersions TABLE (Id INT IDENTITY, VersionId INT)	
	INSERT INTO @tbVersions (VersionId) (select items from [dbo].[SplitTextRS](@Versions,','))
	DECLARE @ComparedVersion INT
	DECLARE @DummyCategoryId INT
	

	SELECT @FeaturedVersionId = FeaturedVersionId, @ComparedVersion = VersionId FROM 
	                    (
						Select Top 1 FeaturedVersionId, SpotlightUrl, VE.VersionId
						from CompareFeaturedCar CFC WITH(NOLOCK)
						INNER JOIN @tbVersions VE ON CFC.VersionId = VE.VersionId							
						WHERE FeaturedVersionId NOT IN(
							SELECT CV.Id FROM CarVersions CV WITH(NOLOCK)
							WHERE CV.CarModelId IN (
							SELECT DISTINCT CarModelId  FROM CarVersions CV WITH(NOLOCK)
							INNER JOIN @tbVersions VE ON CV.Id = VE.VersionId))
						AND 
							1 = CASE WHEN @CategoryId=1 and CFC.IsCompare=1 THEN 1
								WHEN @CategoryId=2 and CFC.IsPriceQuote=1 THEN 1
								ELSE 0 END
						AND CFC.ApplicationId = @PlatformId
						AND CFC.IsActive = 1
						AND	( (CFC.CityId=@cityid  AND ISNULL(CFC.ZoneId,0) =ISNULL(@ZoneId,0) --modified by Ashish
									)-- modified by Ashish
								OR CFC.CityId=-1
							) 				
						ORDER BY VE.Id ASC, CFC.CFC_Id DESC) T

	
END