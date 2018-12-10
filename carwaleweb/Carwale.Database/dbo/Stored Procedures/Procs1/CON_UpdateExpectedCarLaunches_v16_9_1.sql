IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CON_UpdateExpectedCarLaunches_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches_v16_9_1]
GO

	--Modified by:Prashant vishe    On 29 aug 2013 for adding priority related query
--Modified by:Rakesh Yadav On 1 June 2016, made modelId as output parameter and set it if it's not coming as input
--Modifier : Sachin Bharti (7th Sept 2016)
--Purpose : Update carversions table 
CREATE PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches_v16_9_1]        
	@Id    NUMERIC,        
	@ExpectedLaunch VARCHAR(250),
	@UpdatePhotoDateTime VARCHAR(50),        
	@LaunchDate  DATETIME,        
	@EstimatedPriceMin NUMERIC(18,2),        
	@EstimatedPriceMax NUMERIC(18,2),        
	@Url    VARCHAR(100) = NULL,        
	@IsLaunched  BIT,        
	@CWConfidence  TINYINT,        
	@ModelId   VARCHAR(10) = Null OUTPUT,        
	@IsDeleted  BIT,  
	@Priority INT,
	@VersionId	INT = NULL
 AS                
                 
BEGIN       
	DECLARE @IsNew BIT;      
	DECLARE @IsFuturistic BIT = 0;      
	SET @IsNew= (@IsLaunched);       
      
	IF @IsLaunched = 0           
		SET @IsFuturistic=1
            
	UPDATE 
		ExpectedCarLaunches 
	SET	
		LaunchDate = @LaunchDate, ExpectedLaunch = @ExpectedLaunch, EstimatedPriceMin = @EstimatedPriceMin, EstimatedPriceMax = @EstimatedPriceMax,        
		EstimatedPrice = 'Rs.' + REPLACE(Convert(Varchar,@EstimatedPriceMin, 18), '.00', '') + '-' + REPLACE(Convert(Varchar,@EstimatedPriceMax,18), '.00', '') + ' Lakh',        
		IsLaunched = @IsLaunched, CWConfidence = @CWConfidence, UpdatedDate = GETDATE(),IsDeleted =@IsDeleted,Priority=@Priority        
	WHERE 
		Id = @Id      
	--mysql sync
	declare
	@Name varchar(30) =null,
	@CarMakeId numeric(18, 0) =null,
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@MoUpdatedBy numeric(18, 0) =null,
	@Used bit =null,
	@New bit =@IsNew,
	@Indian bit =null,
	@Imported bit =null,
	@Classic bit =null,
	@Futuristic bit =@IsFuturistic,
	@Modified bit =null,
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
	@ReviewCount decimal = null,
	@Summary varchar(max) = null,
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null
		--mysql sync end	  
         
	IF(@VersionId IS NULL) 
    BEGIN
		UPDATE CarModels SET New=@IsNew,Futuristic=@IsFuturistic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)     
		set @UpdateType=20
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
				VALUES('MysqlSync','CON_UpdateExpectedCarLaunches_v16_9_1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH
		--mysql sync end
	END   
	ELSE IF(@VersionId IS NOT NULL) 
	BEGIN    
		UPDATE CarVersions SET New=@IsNew,Futuristic=@IsFuturistic WHERE Id=(SELECT CarVersionId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)     
		-- mysql sync start	
declare   @SegmentId numeric(18, 0)  = null, @BodyStyleId numeric(18, 0)  = null,  @Discontinuation_date datetime =null, @ReplacedByVersionId smallint = null, @VUpdatedBy numeric = null, @VUpdatedOn datetime = null, @CarFuelType tinyint = null, @CarTransmission tinyint = null,  @Environment varchar(150) =null 
set @UpdateType=4
			begin try
exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
	@ID , @Name , @SegmentId , @BodyStyleId , @Used , @IsNew , @IsDeleted , @Indian , @Imported , @IsFuturistic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 	
	
			end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','CON_UpdateExpectedCarLaunches_v16_9_1',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH
--mysql sync end
	END
                
	IF @ModelId IS NOT NULL AND @ModelId != ''            
	BEGIN     
	
	SET @OriginalImgPath = '/cw/expLaunchesCars/' + @ModelId+'.jpg?v='+ @UpdatePhotoDateTime
	SET @HostURL = @Url
	SET @Id=@ModelId
	set @UpdateType=21      
	
		UPDATE CarModels SET OriginalImgPath = '/cw/expLaunchesCars/' + @ModelId+'.jpg?v='+ @UpdatePhotoDateTime, HostURL = @Url ,
		New= @IsNew,Futuristic=@IsFuturistic, IsReplicated = 0 WHERE ID = @ModelId    
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
		--mysql sync end
		     	end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','CON_UpdateExpectedCarLaunches_v16_9_1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
			END CATCH			
	END
	ELSE
	BEGIN
		SET @ModelId= (SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id)	
	END             
END

