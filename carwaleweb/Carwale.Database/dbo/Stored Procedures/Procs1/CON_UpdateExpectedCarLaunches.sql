IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CON_UpdateExpectedCarLaunches]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches]
GO

	            
--Modified by:Prashant vishe    On 29 aug 2013 for adding priority related query
CREATE PROCEDURE [dbo].[CON_UpdateExpectedCarLaunches]        
 @Id    NUMERIC,        
 @ExpectedLaunch VARCHAR(250),
 @UpdatePhotoDateTime VARCHAR(50),        
 @LaunchDate  DATETIME,        
 @EstimatedPriceMin DECIMAL(18,2),        
 @EstimatedPriceMax DECIMAL(18,2),        
 @Url    VARCHAR(100),        
 @IsLaunched  BIT,        
 @CWConfidence  TINYINT,        
 @ModelId   VARCHAR(10) = Null,        
 @IsDeleted         BIT ,  
 @Priority int       
 AS                
                 
BEGIN       
      
DECLARE @IsNew BIT;      
DECLARE @IsFuturistic BIT;      
SET @IsNew= (@IsLaunched);       
      
  IF @IsLaunched = 0      
   begin      
  SET @IsFuturistic=1      
   end      
  else      
   begin       
  SET @IsFuturistic=0      
   end      
            
  UPDATE ExpectedCarLaunches SET LaunchDate = @LaunchDate, ExpectedLaunch = @ExpectedLaunch,                 
  EstimatedPriceMin = @EstimatedPriceMin, EstimatedPriceMax = @EstimatedPriceMax,        
  EstimatedPrice = 'Rs.' + REPLACE(Convert(Varchar,@EstimatedPriceMin, 18), '.00', '') + '-' + REPLACE(Convert(Varchar,@EstimatedPriceMax,18), '.00', '') + ' Lakh',        
  IsLaunched = @IsLaunched, CWConfidence = @CWConfidence, UpdatedDate = GETDATE(),IsDeleted =@IsDeleted,Priority=@Priority        
  WHERE Id = @Id      
      
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
	  
	     
   if(@IsNew=1)    
    begin     
		UPDATE CarModels SET New=@IsNew,Futuristic=@IsFuturistic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WHERE Id=@Id)    
    		set @UpdateType = 20
  end     
 else     
  begin    
  	set @UpdateType = 11
		UPDATE CarModels SET Futuristic=@IsFuturistic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WHERE Id=@Id)     
  end     
          
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
		VALUES('MysqlSync','CON_UpdateExpectedCarLaunches',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
					--mysql sync end
          
  --PRINT 'common'         
                
  IF @ModelId IS NOT NULL AND @ModelId != ''            
   BEGIN              
       -- PRINT 'a'   
	
   SET @SmallPic = @ModelId+'us.jpg'+@UpdatePhotoDateTime 
   SET @LargePic= @ModelId+'ub.jpg'+@UpdatePhotoDateTime 
   SET @HostUrl=@Url
   set @UpdateType = 12
   SET @Id=@ModelId
	        
   UPDATE CarModels SET SmallPic = @ModelId+'us.jpg'+@UpdatePhotoDateTime , LargePic = @ModelId+'ub.jpg'+@UpdatePhotoDateTime , HostURL = @Url ,
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
			 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CON_UpdateExpectedCarLaunches',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH		
   END             
END

