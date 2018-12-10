IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateDiscontinuedModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateDiscontinuedModels]
GO

	         
-- =============================================          
CREATE PROCEDURE [cw].[UpdateDiscontinuedModels]        
 @DiscontinuitionId numeric(18,0),         
 @Discontinuition_date datetime,         
 @ReplacedByModelId smallint,         
 @comment varchar(5000),         
 @OldModelId numeric(18,0)       
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
  update CarModels set	
  		New=0,
		DiscontinuationId=@DiscontinuitionId,
		ReplacedByModelId=@ReplacedByModelId,
		comment=@comment,
		Discontinuation_date=@Discontinuition_date 
	where Id=@OldModelId;  
  
  	--mysql sync
	declare
	@Name varchar(30) =null,
	@CarMakeId numeric(18, 0) =null,
	@IsDeleted bit =null,
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@MoUpdatedBy numeric(18, 0) =null,
	@Used bit =null,
	@New bit =0,
	@Indian bit =null,
	@Imported bit =null,
	@Classic bit =null,
	@Futuristic bit =null,
	@Modified bit =null,
	@Id int =@OldModelId,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT = 5,	
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
	@CV_ID int = null,
	@SelId varchar(8000)=null
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
	@Id ,
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
		VALUES('MysqlSync','UpdateDiscontinuedModels',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@ID,GETDATE(),@UpdateType)
	END CATCH
		--mysql sync end
  DECLARE @VersionId numeric(18,0)
  SELECT @VersionId= Id from CarVersions where CarModelId = @OldModelId
  -- Added by Ravi Koshal to update min and max price in modelCityPrices Table
  Execute UpdateModelPrices  @VersionId,NULL         
              
END 

