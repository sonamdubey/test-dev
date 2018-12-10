IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveCarModels]
GO

	-- =============================================     
-- Author:Prashant Vishe       
-- Create date: <20 sept 2013>     
-- Description: fo saving and updating car models data...   
-- Modified By:Prashant Vishe On 25 sept 2013      
-- Modification:added query for inserting/updating masking name   
-- Modified By:Prashant Vishe On 29 Jan 2014 
-- Modification:added query for saving/updating car model roots. 
-- Modified by Manish on 25-07-2014 commented sp for sync src_keyword table since this activity will be perform by Schedul job.
-- Modified by :Khushaboo Patil on 1 Apr 2015 to update model dependent rules to new modelid added @ReplacedByModelId in insert and update carmodels
-- Modified By Ajay Singh on 7 june 2016 to update Expectedcarlanch table when delete a model
-- =============================================     
CREATE PROCEDURE [dbo].[Con_SaveCarModels] 
  -- Add the parameters for the stored procedure here  
  @Id                   NUMERIC, 
  @Name                 VARCHAR(50)=NULL,
  @MaskedName           VARCHAR(50)=NULL, 
  @CarMakeId            NUMERIC=NULL, 
  @IsDeleted            BIT, 
  @MoUpdatedBy          NUMERIC, 
  @Used                 BIT=NULL, 
  @New                  BIT=NULL, 
  @Indian               BIT=NULL, 
  @Classic              BIT=NULL, 
  @Imported             BIT=NULL, 
  @Modified             BIT=NULL, 
  @Futuristic           BIT=NULL, 
  @IsTopSelling         BIT=NULL, 
  @DiscontinuitionId    NUMERIC(18, 0)=NULL, 
  @Discontinuition_date DATETIME=NULL, 
  @ReplacedByModelId    INT=NULL, 
  @comment              VARCHAR(5000)=NULL, 
  @IsMaskingNameChanged BIT=NULL, 
  @RootId               NUMERIC=NULL, 
  @Generation           TINYINT=NULL, 
  @Upgrade              TINYINT=NULL, 
  @Platform             VARCHAR(500)=NULL, 
  @LaunchDate           DATETIME=NULL, 
  --@ReplacingModelId		INT =NULL,				
  @currentId            NUMERIC =NULL output
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from  
      -- interfering with SELECT statements.  
      SET nocount ON; 
	  	--mysql sync
	declare
	@MoCreatedOn datetime =null,
	@MoUpdatedOn datetime =null,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT =null,
	@Maskingname varchar(50) = null,
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
      -- Insert statements for procedure here
	IF (@IsDeleted = 1)
		BEGIN
			UPDATE CarModels SET IsDeleted = 1 ,MoUpdatedOn = getdate(), MoUpdatedBy = @MoUpdatedBy WHERE ID = @ID
		
		set @UpdateType=2
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
			VALUES('MysqlSync','Con_SaveCarModels',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
		END CATCH			
		--mysql sync end
			UPDATE ExpectedCarLaunches SET IsDeleted = 1 ,UpdatedDate = getdate() WHERE CarModelId = @ID --  Modified By Ajay Singh on 7 june 2016 to update Expectedcarlanch table when delete a model
				
			--	EXEC [ac].[UpdateKeywordsByModelId] @ID,3  -- -- Commented by Manish on 25-07-2014
		END
	ELSE
		BEGIN
			DECLARE @ExistingReplacingModel INT = 0	 
	  
			IF @Id = -1 
				BEGIN 
					INSERT INTO carmodels 
								(name, 
								 carmakeid, 
								 isdeleted, 
								 mocreatedon, 
								 moupdatedby, 
								 used, 
								 new, 
								 indian, 
								 imported, 
								 classic, 
								 modified, 
								 futuristic, 
								 maskingname, 
								 rootid, 
								 platform, 
								 generation, 
								 upgrade, 
								 ModelLaunchDate, 
								 comment,
								 ReplacedByModelId) --added by Khushaboo Patil on 2 Apr 
					VALUES      (@Name, 
								 @CarMakeId, 
								 @IsDeleted, 
								 Getdate(), 
								 @MoUpdatedBy, 
								 @Used, 
								 @New, 
								 @Indian, 
								 @Imported, 
								 @Classic, 
								 @Modified, 
								 @Futuristic, 
								 @MaskedName, 
								 @RootId, 
								 @Platform, 
								 @Generation, 
								 @Upgrade, 
								 @LaunchDate, 
								 @comment,
								 @ReplacedByModelId)
					SET @currentId=Scope_identity()
					--mysql sync
					begin try
					exec [dbo].[SyncCarModelsWithMysql] 						
						@currentId ,
						@Name,
						@CarMakeId,
						@IsDeleted,
						@Used ,
						@New ,
						@Indian ,
						@Imported ,
						@Futuristic ,
						@Classic ,
						@Modified ,	
						@MoUpdatedBy,
						@comment,
						@ReplacedByModelId,
						@MaskedName ,
						@RootId,
						@Platform,
						@Generation,
						@Upgrade,
						@LaunchDate,
						null,
						1
						end try
						BEGIN CATCH
							INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
							VALUES('MysqlSync','Con_SaveCarModels',ERROR_MESSAGE(),'SyncCarModelsWithMysql',@currentId,GETDATE(),1)
						END CATCH	
			
					--EXEC [ac].[UpdateKeywordsByModelId] @currentId,1  -- Commented by Manish on 25-07-2014
			
					DELETE FROM MaskingNameUpdateLog WHERE MaskingName = @MaskedName
				END 
			ELSE 
				BEGIN 
					SELECT @ExistingReplacingModel = ISNULL(ReplacedByModelId,0) FROM CarModels WHERE ID = @Id
					SET @currentId=@Id
					IF @IsMaskingNameChanged = 1 
						BEGIN 
							DECLARE @OldMaskingName VARCHAR(50) 
							SET @OldMaskingName=(SELECT maskingname 
                                       FROM   carmodels 
                                       WHERE  id = @Id) 
							INSERT INTO oldmodelmaskingnames 
										  (modelid, 
										   oldmaskingname, 
										   updatedon, 
										   updatedby) 
							  VALUES      (@Id, 
										   @OldMaskingName, 
										   Getdate(), 
										   @MoUpdatedBy)
							  INSERT INTO MaskingNameUpdateLog (ModelId,
											MaskingName)
									VALUES (@Id,
											@OldMaskingName
											)
						END 			
					UPDATE carmodels 
					SET    name = @Name,						   
						   maskingname = @MaskedName, 
						   used = @Used, 
						   new = @New, 
						   indian = @Indian, 
						   imported = @Imported, 
						   classic = @Classic, 
						   modified = @Modified, 
						   futuristic = @Futuristic, 
						   moupdatedon = Getdate(), 
						   moupdatedby = @MoUpdatedBy, 
						   rootid = @RootId, 
						   platform = @Platform, 
						   generation = @Generation, 
						   upgrade = @Upgrade, 
						   ModelLaunchDate = @LaunchDate, 
						   comment = @comment ,
						   ReplacedByModelId = @ReplacedByModelId
						   
					WHERE  id = @Id
					set @UpdateType=8
					set @maskingname=@MaskedName
					set @ModelLaunchDate=@LaunchDate
			--mysql sync
			begin try
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
					VALUES('MysqlSync','Con_SaveCarModels',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),1)
				END CATCH
		--mysql sync end
					DELETE FROM MaskingNameUpdateLog WHERE MaskingName = @MaskedName
					--	EXEC [ac].[UpdateKeywordsByModelId]	@Id,2   -- Commented by Manish on 25-07-2014
				END 
        
				--modified by :Khushaboo Patil on 1 Apr 2015 to update model dependent rules to new modelid
				--DECLARE @ExistingReplacingModel INT
				--SELECT @ExistingReplacingModel = ReplacedByModelId FROM CarModels WHERE ID = @currentId
	  
			IF ISNULL(@ReplacedByModelId,0) > 0 AND (@ExistingReplacingModel <> @ReplacedByModelId)	
				BEGIN
					EXEC Con_UpdateReplacingModelRules  @currentId  , @ReplacedByModelId
				END
	  
	  
			IF EXISTS(SELECT modelid FROM   con_topsellingcars  WHERE  modelid = @Id) 
				BEGIN 
					UPDATE con_topsellingcars 
					SET    status = @IsTopSelling 
					WHERE  modelid = @Id; 
				END 
			ELSE 
				BEGIN 
					INSERT INTO con_topsellingcars 
								(modelid, 
								 entrydate, 
								 hosturl, 
								 imgpath, 
								 status, 
								 sortorder, 
								 isreplicated) 
					VALUES      (@currentId, 
								 Getdate(), 
								 NULL, 
								 NULL, 
								 @IsTopSelling, 
								 NULL, 
								 NULL) 
				END 
			
			
			set @UpdateType=9
			IF ( ( @Id <> -1 ) AND ( @New = 0 ) ) 
				BEGIN 
					UPDATE carmodels 
					SET    new = 0, 
						   discontinuationid = @DiscontinuitionId, 
						   --replacedbymodelid = @ReplacedByModelId, 
						   --comment = @comment, 
						   discontinuation_date = @Discontinuition_date 
					WHERE  id = @Id; 					
					set @new=0					
				END 
			ELSE 
				BEGIN 
					UPDATE carmodels 
					SET    new = 1, 
						   -- comment = NULL, 
						   discontinuation_date = NULL, 
						   --replacedbymodelid = NULL, 
						   discontinuationid = NULL 
					WHERE  id = @Id 
					set @new=1
					set @Discontinuition_date=null
					set @DiscontinuitionId=null
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
				@SelId
				end try
				BEGIN CATCH
					INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
					VALUES('MysqlSync','Con_SaveCarModels',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
				END CATCH
		--mysql sync end
			
		END
		print @currentId
  END

