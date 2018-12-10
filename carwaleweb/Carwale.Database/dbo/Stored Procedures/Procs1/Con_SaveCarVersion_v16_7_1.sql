IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveCarVersion_v16_7_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveCarVersion_v16_7_1]
GO

	--Modified By:Prashant Vishe On 30 Aug 2013 added queries for updating Car Versions data....   
--Modified By:Prashant vishe On 16 sept 2013 added inserting/updating queries for column createdOn     
--Modified By:Prashant Vishe on 25 sept 2013  added query for inserting/updating MaskingName 
-- Modified by Manish on 25-07-2014 commented sp for sync src_keyword table since this activity will be perform by Schedul job.
--Modified by Manish on 07-10-2014 taken Scope_identity() value for CarVersions in a Variable and used that values as New Version Id
--Modified by Piyush on 5/13/2016 called UpdateModelPrices to update model min and max prices in carmodels table 
-- exec [dbo].[Con_SaveCarVersion_v16_7_1] 4042,'35 TDI Premium + Sunroof','35-tdi-premium-sunroof',552,2,1,2,1,1,1,1,0,0,0,0,0,11,GETDATE(),3,null,0,getdate(),null,null
-- exec [dbo].[Con_SaveCarVersion_v16_7_1] 3620,'35 TDI Premium','35 TDI Premium',552,2,1,null,null,1,1,1,0,0,0,1,0,null,null,3,null,null,null,null,null
-- Modified by Sachin Bharti on 5th July 2016
-- Purpose : Call [dbo].[UpdateAvgPriceByVersionId] to update average prices when version made active or inactive
CREATE PROCEDURE [dbo].[Con_SaveCarVersion_v16_7_1]
	@ID NUMERIC
	,@Name VARCHAR(50) = NULL
	,@MaskingName VARCHAR(50) = NULL
	,@CarModelId NUMERIC = NULL
	,@SegmentId NUMERIC = NULL
	,@BodyStyleId NUMERIC = NULL
	,@FuelType NUMERIC = NULL
	,@Transmission NUMERIC = NULL
	,@Used BIT = NULL
	,@New BIT = NULL
	,@Indian BIT = NULL
	,@Imported BIT = NULL
	,@Classic BIT = NULL
	,@Modified BIT = NULL
	,@Futuristic BIT = NULL
	,@IsDeleted BIT
	,@SubSegmentId NUMERIC = NULL
	,@CreatedOn DATETIME = NULL
	,@UpdatedBy NUMERIC
	,@Discontinuedname VARCHAR(100) = NULL
	,@IsMaskingNameChanged BIT = NULL
	,@LaunchDate DATETIME = NULL
	,@DiscontinueDate DATETIME = NULL
	,@CV_ID INT OUTPUT 
AS
BEGIN
	SET NOCOUNT ON
	declare
		@CarMakeId numeric(18, 0) =null,
		@MoCreatedOn datetime =null,
		@MoUpdatedOn datetime =null,
		@MoUpdatedBy numeric(18, 0) =null,		
		@SmallPic varchar(200) =null,
		@LargePic varchar(200) =null,
		@HostUrl varchar(100) =null,
		@UpdateType INT =10,
		@DiscontinuitionId int =null,
		@ReplacedByModelId smallint =null,
		@comment Varchar(max) = null,
		@Discontinuition_date datetime = null,
		@RootId smallint =null,
		@Platform varchar(50)=null, 
		@Generation tinyint=null, 
		@Upgrade tinyint =null, 
		@ModelLaunchDate datetime =null,
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
		@SelId varchar(8000)=null
		--mysql sync end
	--SET @CurrentId = -1
		-- mysql sync start	
declare  @Discontinuation_date datetime =null,@ReplacedByVersionId smallint = null, @VUpdatedBy numeric = null, @VUpdatedOn datetime = null, @CarFuelType tinyint = null, @Environment varchar(150) =null , @CarTransmission tinyint = null
	IF (@IsDeleted = 1)
	BEGIN
		declare @TempDate datetime =Getdate()
		UPDATE CarVersions SET IsDeleted = 1 ,VUpdatedOn = @TempDate, VUpdatedBy = @UpdatedBy WHERE ID = @ID
		EXECUTE [dbo].[UpdateModelPrices_v16_6_1]  @Id,null,@UpdatedBy--Modified by Sachin Bharti on 5th July 2016
  		set @UpdateType=2
		set @IsDeleted=1
		set @VUpdatedBy=@UpdatedBy
		set @VUpdatedOn=@TempDate
	begin try
		exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
			@ID , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 
		
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','Con_SaveCarVersion_v16_7_1',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
	END
	ELSE
	BEGIN         
	IF @ID = - 1
	BEGIN
		INSERT INTO carversions (
			NAME
			,carmodelid
			,segmentid
			,bodystyleid
			,carfueltype
			,cartransmission
			,used
			,new
			,indian
			,imported
			,classic
			,modified
			,maskingname
			,futuristic
			,isdeleted
			,subsegmentid
			,vcreatedon
			,vcreatedby
			,Discontinuation_date
			,LaunchDate
			)
		VALUES (
			@Name
			,@CarModelId
			,@SegmentId
			,@BodyStyleId
			,@FuelType
			,@Transmission
			,@Used
			,@New
			,@Indian
			,@Imported
			,@Classic
			,@Modified
			,@MaskingName
			,@Futuristic
			,@IsDeleted
			,@SubSegmentId
			,@CreatedOn
			,@UpdatedBy
			,@DiscontinueDate
			,@LaunchDate
			)
			
		SET @CV_ID = SCOPE_IDENTITY()  -- Used Scope Identity in place of Identity on 07-10-2014
		begin try
		--syncing with mysql
			exec SyncCarVersionsWithMysql @CV_ID,@Name,@CarModelId,@SegmentId,@BodyStyleId,@IsDeleted,@Used,@New,@Indian,@Imported,@Futuristic,@Classic,@Modified,
			@FuelType,@Transmission,@CreatedOn,@DiscontinueDate,@MaskingName,@SubSegmentId,@UpdatedBy,@LaunchDate;
 		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarVersion_v16_7_1',ERROR_MESSAGE(),'SyncCarVersionsWithMysql',@CV_ID,GETDATE(),null)
		END CATCH	
		EXECUTE [dbo].[UpdateModelPrices_v16_6_1]  @Id,null,@UpdatedBy--Modified by Sachin Bharti on 5th July 2016
		
		INSERT INTO RecommendCars (
									Makeid
									,Makename
									,Modelid
									,Modelname
									,Versionid
									,Versionname
			                      ) 
								 (
									SELECT CMO.CarMakeId
									,CM.NAME
									,CMO.ID
									,CMO.NAME
									,CV.Id
									,CV.NAME FROM CarMakes CM WITH (NOLOCK)
									,CarModels CMO WITH (NOLOCK)
									,CarVersions CV  WITH (NOLOCK) WHERE CM.Id = CMO.CarMakeId
									AND CV.CarModelId = CMO.id
									AND CV.Id = @CV_ID
			                      )
	END
	ELSE
	BEGIN
		IF @IsMaskingNameChanged = 1
		BEGIN
			DECLARE @OldMaskingName VARCHAR(50)
			SET @OldMaskingName = (
					SELECT maskingname
					FROM carversions WITH (NOLOCK)
					WHERE id = @Id
					)
			INSERT INTO oldversionmaskingnames (
				versionid
				,oldmaskingname
				,updatedon
				,updatedby
				)
			VALUES (
				@Id
				,@OldMaskingName
				,Getdate()
				,@UpdatedBy
				)
		END
		declare @curdate datetime = getdate();
		
		UPDATE carversions
		SET NAME = @Name
			,segmentid = @SegmentId
			,subsegmentid = @SubSegmentId
			,bodystyleid = @BodyStyleId
			,carfueltype = @FuelType
			,cartransmission = @Transmission
			,used = @Used
			,new = @New
			,indian = @Indian
			,imported = @Imported
			,classic = @Classic
			,modified = @Modified
			,futuristic = @Futuristic
			,vupdatedon = @curdate
			,vupdatedby = @UpdatedBy
			,maskingname = @MaskingName
			,Discontinuation_date = @DiscontinueDate
			,LaunchDate = @LaunchDate
		WHERE id = @Id
	
  	set @UpdateType=3
	set @CarFuelType=@FuelType
	set @CarTransmission=@Transmission
	set @Discontinuation_date=@DiscontinueDate
	set @VUpdatedOn = @curdate
	set @VUpdatedBy = @UpdatedBy
	begin try
				exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
					@ID , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 	
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarVersion_v16_7_1',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@ID,GETDATE(),@UpdateType)
		END CATCH	
		IF ((@New = 0 OR @New = 1) AND @IsDeleted = 0)
		BEGIN
			EXECUTE [dbo].[UpdateModelPrices_v16_6_1]  @Id,null,@UpdatedBy--Modified by Sachin Bharti on 5th July 2016
		END
		
	END
	
	IF (
			(
				SELECT carversionid_top
				FROM carmodels WITH(NOLOCK) 
				WHERE id = @CarModelId
				) IS NULL
			)
		UPDATE carmodels
		SET carversionid_top = (
				CASE @ID
					WHEN - 1
						THEN @CV_ID
					ELSE @ID
					END
				)
			,subsegmentid = @SubSegmentId
		WHERE id = @CarModelId
		
		--mysql sync
			set @UpdateType =10;
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
				@SelId,
				@CarModelId
					--mysql sync end
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','Con_SaveCarVersion_v16_7_1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@CarModelId,GETDATE(),@UpdateType)
		END CATCH	
	END
END

