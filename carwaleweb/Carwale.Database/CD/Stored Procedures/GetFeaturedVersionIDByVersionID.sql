IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetFeaturedVersionIDByVersionID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetFeaturedVersionIDByVersionID]
GO

	-- =============================================
-- Author:		Supriya
-- Create date: 5/8/2014
-- Description:	SP to get the featured car and track the sponsored version on car comparison page 

-- Modifications:
-- Last Modified By : Satish Sharma On 18-Aug-2014; Commented sponsored car logging.	
-- Last Modified By : Supriya K on 19/8/2014 added CategoryId as i/p parameter to get featured car based on category passed.
-- Last Modified By : Supriya K on 27/8/2014 to filter data based on platformId passed
-- Last Modified By : Supriya K on 12/9/2014 to add isActive filter
-- Declare @FeaturedVersionId INT exec [CD].[GetFeaturedVersionIDByVersionID] '2524,2197',1,1,@FeaturedVersionId out select @FeaturedVersionId
--Modified By : Sadhana Upadhyay on 16 Sept 2014 to get featured bike using WebApi
-- =============================================
CREATE PROCEDURE [CD].[GetFeaturedVersionIDByVersionID] 
@Versions VARCHAR(255),
@CategoryId INT=1,
@PlatformId INT=1,
@FeaturedVersionId INT=NULL OUTPUT
AS
BEGIN
	DECLARE @tbVersions TABLE (Id INT IDENTITY, VersionId INT)
	
	INSERT INTO @tbVersions (VersionId) (select items from [dbo].[SplitTextRS](@Versions,','))

	 DECLARE @ComparedVersion INT

	DECLARE @DummyCategoryId INT
	
	IF(@PlatformId = 1)		
	BEGIN
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
							ORDER BY VE.Id ASC, CFC.CFC_Id DESC) T
	END

	ELSE IF(@PlatformId = 2)
	BEGIN
		SELECT @FeaturedVersionId = FeaturedVersionId, @ComparedVersion = VersionId FROM 
							(
							Select Top 1 FeaturedVersionId, SpotlightUrl, VE.VersionId
							from CompareFeaturedCar CFC WITH(NOLOCK)
							INNER JOIN @tbVersions VE ON CFC.VersionId = VE.VersionId							
							WHERE FeaturedVersionId NOT IN(
								SELECT CV.Id FROM BikeVersions CV WITH(NOLOCK)
								WHERE CV.BikeModelId IN (
								SELECT DISTINCT BikeModelId  FROM BikeVersions CV WITH(NOLOCK)
								INNER JOIN @tbVersions VE ON CV.Id = VE.VersionId))
							AND 
								1 = CASE WHEN @CategoryId=1 and CFC.IsCompare=1 THEN 1
									--WHEN @CategoryId=2 and CFC.IsPriceQuote=1 THEN 1
									ELSE 0 END
							AND CFC.ApplicationId = @PlatformId	AND CFC.IsActive = 1	                 				
							ORDER BY VE.Id ASC, CFC.CFC_Id DESC) T
	END
													 
	--if( ISNULL(@FeaturedVersionId, 0) > 0 )
	--BEGIN
	--	EXEC CompareCars_Sponsored_Logs_SP @ComparedVersion, @FeaturedVersionId
 --   END
END
