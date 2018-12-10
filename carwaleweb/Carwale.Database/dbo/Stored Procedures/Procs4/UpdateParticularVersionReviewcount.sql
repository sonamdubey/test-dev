IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateParticularVersionReviewcount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateParticularVersionReviewcount]
GO

	
---Created By : Manish Chourasiya on 18-09-2014
--Description : for correction of review count in CarVersions  table.
CREATE PROCEDURE [dbo].[UpdateParticularVersionReviewcount]
    @VersionId INT
AS  
BEGIN  
 
 DECLARE @ReviewCount FLOAT 
   
 DECLARE @SumRating FLOAT
 DECLARE @SumStyleR FLOAT
 DECLARE @SumComfortR FLOAT
 DECLARE @SumPerformanceR FLOAT
 DECLARE @SumValueR FLOAT
 DECLARE @SumFuelEconomyR FLOAT
   
 DECLARE @AvgRating FLOAT
 DECLARE @AvgStyleR FLOAT  
 DECLARE @AvgComfortR FLOAT
 DECLARE @AvgPerformanceR FLOAT
 DECLARE @AvgValueR FLOAT
 DECLARE @AvgFuelEconomyR FLOAT   
   

   
 IF @VersionId <> -1  
 BEGIN  
      
  SELECT   
   @SumRating = IsNull(SUM(OverallR), 0),   
   @SumStyleR = IsNull(SUM(StyleR), 0),   
   @SumComfortR = IsNull(SUM(ComfortR), 0),   
   @SumPerformanceR = IsNull(SUM(PerformanceR), 0),   
   @SumValueR = IsNull(SUM(ValueR), 0),   
   @SumFuelEconomyR = IsNull(SUM(FuelEconomyR), 0),   
   @ReviewCount  = COUNT(Id)  
  FROM CustomerReviews  WITH (NOLOCK)
  WHERE VersionId = @VersionId AND IsActive = 1 AND IsVerified = 1  
  GROUP BY VersionId  
  
  IF @@RowCount > 0  
  BEGIN  
	   SET @AvgRating = @SumRating/@ReviewCount  
	   SET @AvgStyleR = @SumStyleR/@ReviewCount  
	   SET @AvgComfortR = @SumComfortR/@ReviewCount  
	   SET @AvgPerformanceR = @SumPerformanceR/@ReviewCount  
	   SET @AvgValueR = @SumValueR/@ReviewCount  
	   SET @AvgFuelEconomyR = @SumFuelEconomyR/@ReviewCount  
     
   UPDATE CarVersions   
   SET   
    ReviewRate = @AvgRating,   
    Looks = @AvgStyleR,   
    Comfort = @AvgComfortR,   
    Performance = @AvgPerformanceR,   
    ValueForMoney = @AvgValueR,   
    FuelEconomy = @AvgFuelEconomyR,   
    ReviewCount = @ReviewCount  
   WHERE ID = @VersionId  
  END  
  ELSE  
  BEGIN  
   UPDATE CarVersions   
   SET   
    ReviewRate = 0.0,  
    Looks = 0.0,   
    Comfort = 0.0,   
    Performance = 0.0,   
    ValueForMoney = 0.0,   
    FuelEconomy = 0.0,    
    ReviewCount = 0  
   WHERE ID = @VersionId  
  END  
   
 END  
  
  
END  
 
/****** Object:  Trigger [dbo].[TrigUpdateReviews]    Script Date: 09/19/2014 16:54:39 ******/
SET ANSI_NULLS ON
