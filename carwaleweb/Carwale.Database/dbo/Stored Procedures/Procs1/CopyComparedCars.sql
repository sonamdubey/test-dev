IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CopyComparedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CopyComparedCars]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 2-May-2012
-- Description:	Copy compared cars from one version into another version
-- =============================================
CREATE PROCEDURE [dbo].[CopyComparedCars]
	-- Add the parameters for the stored procedure here
	@NewVersion		INT, 
	@FeaturedCarId	INT,
	@IsCompare		BIT,
	@IsNewSearch	BIT,
	@IsRecommend	BIT,
	@IsResearch		BIT,
	@SpotLightURL	VARCHAR(150)
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @FeaturedVersionId int,@NewFeaturedVersionId int
    -- Insert statements for procedure here
    SET @FeaturedVersionId = @FeaturedCarId
	SET @NewFeaturedVersionId = @NewVersion
    
	INSERT INTO CompareFeaturedCar(VersionId,FeaturedVersionId,IsActive,LocationId,
				IsCompare,IsNewSearch,IsRecommend,IsResearch,SpotlightUrl)
	SELECT VersionId,@NewFeaturedVersionId,IsActive,
			LocationId,IsCompare,IsNewSearch,
			IsRecommend,IsResearch,SpotlightUrl
	FROM CompareFeaturedCar
	WHERE FeaturedVersionId=@FeaturedVersionId
	
 --(start)added by amit verma on 29 aug 2013 to manage tracking code for featured cars	
 IF NOT EXISTS(SELECT * FROM FeaturedCarsTrackingCode WHERE VersionId = @FeaturedCarId)
 BEGIN
	DECLARE @ModelId INT
	DECLARE @MakeId INT
	
	SELECT @MakeId = MakeId,@ModelId=ModelId FROM CD.vwMMV WHERE VersionId = @FeaturedCarId
	
	INSERT INTO FeaturedCarsTrackingCode (VersionId,ModelID,MakeId)
	VALUES(@FeaturedCarId,@ModelId,@MakeId)
 END
 --(end)added by amit verma on 29 aug 2013 to manage tracking code for featured cars
END

