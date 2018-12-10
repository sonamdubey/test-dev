IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateParticularModelReviewcount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateParticularModelReviewcount]
GO

	---Created By : Manish Chourasiya on 18-09-2014
--Description : for correction of review count in Carmodels  table.
CREATE PROCEDURE [dbo].[UpdateParticularModelReviewcount]
  @ModelId INT
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
   
 
 SELECT   
  @SumRating = IsNull(SUM(OverallR), 0),   
  @SumStyleR = IsNull(SUM(StyleR), 0),   
  @SumComfortR = IsNull(SUM(ComfortR), 0),   
  @SumPerformanceR = IsNull(SUM(PerformanceR), 0),   
  @SumValueR = IsNull(SUM(ValueR), 0),   
  @SumFuelEconomyR = IsNull(SUM(FuelEconomyR), 0),   
  @ReviewCount  = COUNT(Id)  
 FROM CustomerReviews  WITH (NOLOCK)
 WHERE ModelId = @ModelId AND IsActive = 1 AND IsVerified = 1  
 GROUP BY ModelId  
  
  	--mysql sync
	declare
	@Name varchar(30) =null,
	@CarMakeId numeric(18, 0) =null,
	@IsDeleted bit =null,
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@MoUpdatedBy numeric(18, 0) =null,
	@Used bit =null,
	@New bit =null,
	@Indian bit =null,
	@Imported bit =null,
	@Classic bit =null,
	@Futuristic bit =null,
	@Modified bit =null,
	@Id int =null,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT =null,
	@DiscontinuitionId int =null,
	@ReplacedByModelId smallint =null,
	@comment Varchar(max) = null,
	@Discontinuition_date datetime = null,
	@Maskingname varchar(50) = null,
	@RootId smallint =null,
	@Platform varchar(50)=null, 
	@Generation tinyint=null, 
	@Upgrade tinyint =null, 
	@ModelLaunchDate datetime =null,
	@SubSegmentId int = null,
	@CarVersionID_Top int =null,
	@IsSolidColor bit =null,
	@IsMetallicColor bit =null,
	@MinPrice int =null, 
	@MaxPrice int =null,
	@ReviewRate decimal = null,   
	@Looks decimal = null,   
	@Comfort decimal = null,   
	@Performance decimal = null,   
	@ValueForMoney decimal = null,   
	@FuelEconomy decimal = null,   
	@Summary varchar(max) = null,
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null
		--mysql sync end
 IF @@RowCount > 0  
 BEGIN  
  SET @AvgRating = @SumRating/@ReviewCount  
  SET @AvgStyleR = @SumStyleR/@ReviewCount  
  SET @AvgComfortR = @SumComfortR/@ReviewCount  
  SET @AvgPerformanceR = @SumPerformanceR/@ReviewCount  
  SET @AvgValueR = @SumValueR/@ReviewCount  
  SET @AvgFuelEconomyR = @SumFuelEconomyR/@ReviewCount  
   
  set @Id=@ModelId
  set @UpdateType=16
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
   set @ReviewRate = @AvgRating
   set @Looks = @AvgStyleR  
   set @Comfort = @AvgComfortR
   set @Performance = @AvgPerformanceR
   set @ValueForMoney = @AvgValueR
   set @FuelEconomy = @AvgFuelEconomyR
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
   set @ReviewRate = 0.0
   set @Looks = 0.0
   set @Comfort = 0.0
   set @Performance = 0.0
   set @ValueForMoney = 0.0
   set @FuelEconomy = 0.0
   set @ReviewCount = 0
 END  
 
 
		begin try
		--mysql sync
	exec [dbo].[SyncCarModelsWithMysqlUpdate]	
		@Name ,
		@CarMakeId,
		@IsDeleted,
		@MoCreatedOn,
		@MoUpdatedOn,
		@MoUpdatedBy,
		@Used ,
		@New ,
		@Indian ,
		@Imported ,
		@Classic ,
		@Futuristic ,
		@Modified ,
		@Id,
		@SmallPic ,
		@LargePic ,
		@HostUrl ,
		@UpdateType ,
		@DiscontinuitionId  ,
		@ReplacedByModelId  ,
		@comment,
		@Discontinuition_date ,
		@Maskingname ,
		@RootId  ,
		@Platform , 
		@Generation , 
		@Upgrade  , 
		@ModelLaunchDate  ,
		@SubSegmentId ,
		@CarVersionID_Top  ,
		@IsSolidColor  ,
		@IsMetallicColor  ,
		@MinPrice  , 
		@MaxPrice  ,
		@ReviewRate ,   
		@Looks ,   
		@Comfort ,   
		@Performance ,   
		@ValueForMoney ,   
		@FuelEconomy ,   
		@ReviewCount ,
		@Summary,
		@OriginalImgPath ,
		@XLargePic ,
		@CV_ID,
		@SelId,
		@CarModelId
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateParticularModelReviewcount',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@ID,GETDATE(),@UpdateType)
	END CATCH
	--mysql sync end
 
  
END  
 

