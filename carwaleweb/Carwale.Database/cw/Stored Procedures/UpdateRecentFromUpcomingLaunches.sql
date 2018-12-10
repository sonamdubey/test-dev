IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateRecentFromUpcomingLaunches]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateRecentFromUpcomingLaunches]
GO

	-- =============================================      
-- Author:  <Prashant Vishe>      
-- Create date: <29 Aug 2013>      
-- Description: <Used for updating recent launches to upcoming cars>      
-- Modified by Manish Chourasiya on 05-05-2014 for updating min and max price of the models in ModelMetroCityPrices table
-- =============================================      
CREATE PROCEDURE [cw].[UpdateRecentFromUpcomingLaunches]       
 -- Add the parameters for the stored procedure here      
   @SelId nvarchar(500),      
   @LaunchDate datetime      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
    -- Insert statements for procedure here ;  
          
 UPDATE ExpectedCarLaunches SET priority=null,IsLaunched=1,LaunchDate=@LaunchDate WHERE Id IN(select * from dbo.SplitTextRS(@SelId,','))      
       
 UPDATE CarModels SET New=1,Futuristic=0 WHERE Id In (SELECT CarModelId FROM  ExpectedCarLaunches WHERE Id IN(select * from dbo.SplitTextRS(@SelId,',')) )    
          
	--mysql sync
	declare
	@Name varchar(30) =null,
	@CarMakeId numeric(18, 0) =null,
	@IsDeleted bit =null,
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@MoUpdatedBy numeric(18, 0) =null,
	@Used bit =null,
	@New bit =1,
	@Indian bit =null,
	@Imported bit =null,
	@Classic bit =null,
	@Futuristic bit =0,
	@Modified bit =null,
	@Id int =null,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT =6,
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
	@ReviewCount decimal = null,
	@Summary varchar(max) = null,
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null
		--mysql sync end
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
	@SelId
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateRecentFromUpcomingLaunches',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@SelId,GETDATE(),@UpdateType)
	END CATCH
		--mysql sync end
	
-------------------------------------------------------------------------------------------------------------
------block added  by Manish Chourasiya on 05-05-2014 for updating min and max price of the models in ModelMetroCityPrices table
	DECLARE @CarVersionId INT
	DECLARE @WhileLoopControl INT=0
	DECLARE @WhileLoopCount INT	  
	DECLARE @TblModelIds TABLE (Id INT IDENTITY(1,1),CarModelId INT)
	INSERT INTO @TblModelIds(CarModelId)
	SELECT Items from dbo.SplitTextRS(@SelId,',')
	SELECT @WhileLoopCount=COUNT(Id) FROM @TblModelIds
	WHILE (@WhileLoopControl<@WhileLoopCount)
	BEGIN 
			SELECT TOP 1 @CarVersionId=V.ID 
			FROM CarVersions AS V WITH (NOLOCK) 
			JOIN @TblModelIds AS T ON T.CarModelId=V.CarModelId
			WHERE T.ID=@WhileLoopControl 
			EXEC [dbo].[UpdateModelPrices]  @CarVersionId =@CarVersionId,
	                                        @CityId =NULL   
			
			SET @WhileLoopControl =@WhileLoopControl +1
	
	END 
	   
END 

