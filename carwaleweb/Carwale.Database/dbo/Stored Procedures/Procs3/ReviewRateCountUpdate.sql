IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ReviewRateCountUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ReviewRateCountUpdate]
GO

	--- Modify by Manish Chourasiya on 19-09-2014 since this activity will be done by trigger on CustomerReviews table.
CREATE PROCEDURE [dbo].[ReviewRateCountUpdate]  
@ID NUMERIC(18,0)  
AS  
BEGIN  
  
  SET NOCOUNT ON;
 /*DECLARE @ModelId NUMERIC(18,0)  
 DECLARE @VersionId NUMERIC(18,0)  
   
 DECLARE @ReviewCount NUMERIC(18,0)   
   
 DECLARE @SumRating DECIMAL(18,2)  
 DECLARE @SumStyleR DECIMAL(18,2)
 DECLARE @SumComfortR DECIMAL(18,2)  
 DECLARE @SumPerformanceR DECIMAL(18,2)  
 DECLARE @SumValueR DECIMAL(18,2)
 DECLARE @SumFuelEconomyR DECIMAL(18,2)
   
 DECLARE @AvgRating DECIMAL(18,2)
 DECLARE @AvgStyleR DECIMAL(18,2)  
 DECLARE @AvgComfortR DECIMAL(18,2) 
 DECLARE @AvgPerformanceR DECIMAL(18,2)
 DECLARE @AvgValueR DECIMAL(18,2) 
 DECLARE @AvgFuelEconomyR DECIMAL(18,2)   
   
 SELECT @ModelId = ModelId, @VersionId = VersionId   
 FROM CustomerReviews   
 WHERE ID = @ID  
   
 IF @VersionId <> -1  
 BEGIN  
     
  --Update Versions  
  SELECT   
   @SumRating = IsNull(SUM(OverallR), 0),   
   @SumStyleR = IsNull(SUM(StyleR), 0),   
   @SumComfortR = IsNull(SUM(ComfortR), 0),   
   @SumPerformanceR = IsNull(SUM(PerformanceR), 0),   
   @SumValueR = IsNull(SUM(ValueR), 0),   
   @SumFuelEconomyR = IsNull(SUM(FuelEconomyR), 0),   
   @ReviewCount  = COUNT(Id)  
  FROM CustomerReviews  
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
  
 --Update Models  
 SELECT   
  @SumRating = IsNull(SUM(OverallR), 0),   
  @SumStyleR = IsNull(SUM(StyleR), 0),   
  @SumComfortR = IsNull(SUM(ComfortR), 0),   
  @SumPerformanceR = IsNull(SUM(PerformanceR), 0),   
  @SumValueR = IsNull(SUM(ValueR), 0),   
  @SumFuelEconomyR = IsNull(SUM(FuelEconomyR), 0),   
  @ReviewCount  = COUNT(Id)  
 FROM CustomerReviews  
 WHERE ModelId = @ModelId AND IsActive = 1 AND IsVerified = 1  
 GROUP BY ModelId  
  
 IF @@RowCount > 0  
 BEGIN  
  SET @AvgRating = @SumRating/@ReviewCount  
  SET @AvgStyleR = @SumStyleR/@ReviewCount  
  SET @AvgComfortR = @SumComfortR/@ReviewCount  
  SET @AvgPerformanceR = @SumPerformanceR/@ReviewCount  
  SET @AvgValueR = @SumValueR/@ReviewCount  
  SET @AvgFuelEconomyR = @SumFuelEconomyR/@ReviewCount  
     
  UPDATE CarModels   
  SET   
   ReviewRate = @AvgRating,   
   Looks = @AvgStyleR,   
   Comfort = @AvgComfortR,   
   Performance = @AvgPerformanceR,   
   ValueForMoney = @AvgValueR,   
   FuelEconomy = @AvgFuelEconomyR,   
   ReviewCount = @ReviewCount  
  WHERE ID = @ModelId  
 END  
 ELSE  
 BEGIN  
  UPDATE CarModels   
  SET   
   ReviewRate = 0.0,   
   Looks = 0.0,   
   Comfort = 0.0,   
   Performance = 0.0,   
   ValueForMoney = 0.0,   
   FuelEconomy = 0.0,   
   ReviewCount = 0  
  WHERE ID = @ModelId  
 END  */
  
END  
 