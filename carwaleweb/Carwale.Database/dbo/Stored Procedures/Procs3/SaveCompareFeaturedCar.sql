IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveCompareFeaturedCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveCompareFeaturedCar]
GO

	CREATE PROCEDURE [dbo].[SaveCompareFeaturedCar]      
@VersionId INT,      
@FeatureVersionId INT,      
@IsCompare BIT,  
@IsNewSearch BIT,  
@IsRecommend BIT,  
@IsResearch BIT,  
@SpotlightUrl VARCHAR(150),
@IsPriceQuote BIT = 0,
@ApplicationId	TINYINT = 1
  
AS      
BEGIN      
      
 DECLARE @IsActive BIT      
 SELECT @IsActive = IsActive       
        FROM CompareFeaturedCar       
        WHERE VersionId = @VersionId AND FeaturedVersionId = @FeatureVersionId       
      
 IF @IsActive IS NULL       
 BEGIN      
        
  INSERT INTO CompareFeaturedCar      
  (VersionId, FeaturedVersionId,IsActive,IsCompare,IsNewSearch,IsRecommend,IsResearch,SpotlightUrl,IsPriceQuote,ApplicationId)      
  VALUES      
  (@VersionId, @FeatureVersionId,1,@IsCompare,@IsNewSearch,@IsRecommend,@IsResearch,@SpotlightUrl,@IsPriceQuote,@ApplicationId)      

 END      
     
 IF @IsActive = 0       
 BEGIN      
        
  UPDATE CompareFeaturedCar      
  SET IsActive = 1 , IsCompare = @IsCompare, IsNewSearch = @IsNewSearch,IsRecommend = @IsRecommend, IsResearch =@IsResearch ,SpotlightUrl =@SpotlightUrl, IsPriceQuote =  @IsPriceQuote
  WHERE VersionId = @VersionId AND FeaturedVersionId = @FeatureVersionId       
       
 END      
 
 
 --(start)added by amit verma on 29 aug 2013 to manage tracking code for featured cars
 IF NOT EXISTS(SELECT * FROM FeaturedCarsTrackingCode WHERE VersionId = @FeatureVersionId)
 BEGIN
	DECLARE @ModelId INT
	DECLARE @MakeId INT
	
	SELECT @MakeId = MakeId,@ModelId=ModelId FROM CD.vwMMV WHERE VersionId = @FeatureVersionId
	
	INSERT INTO FeaturedCarsTrackingCode (VersionId,ModelID,MakeId)
	VALUES(@FeatureVersionId,@ModelId,@MakeId)
 END
 --(end)added by amit verma on 29 aug 2013 to manage tracking code for featured cars     
END
