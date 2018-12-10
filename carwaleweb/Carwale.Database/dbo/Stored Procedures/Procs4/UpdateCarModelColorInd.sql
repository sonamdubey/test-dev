IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarModelColorInd]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarModelColorInd]
GO

	-- =============================================
-- Author:		<Jitendra>
-- Create date: <10/5/2013>
-- Description:	<This Sp is used to update car model solid and metalic color indicator>
-- =============================================
/* [dbo].[UpdateCarModelColorInd] 10,493 */
CREATE PROCEDURE [dbo].[UpdateCarModelColorInd]
	-- Add the parameters for the stored procedure here
	@ModelId	INT,
	@CityId     INT,
	@SolidColor BIT,
	@MetallicColor BIT
 AS
	BEGIN		
		
	   UPDATE CarModels SET isSolidColor=@SolidColor,isMetallicColor=@MetallicColor WHERE ID=@ModelId
	   
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
	@Id int =@ModelId,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT =14,
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
	@IsSolidColor bit =@SolidColor,
	@IsMetallicColor bit =@MetallicColor,
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
		begin try
		set @UpdateType =14
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
				VALUES('MysqlSync','UpdateCarModelColorInd',ERROR_MESSAGE(),'[SyncCarModelsWithMysqlUpdate]',@ModelId,GETDATE(),@UpdateType)
			END CATCH		
	--mysql sync end
	   DECLARE @delColor BIT
	   
	   IF(NOT (@SolidColor=1 AND @MetallicColor=1))
		BEGIN
			SET @delColor = CASE WHEN 	@SolidColor=1 THEN 1 ELSE 0 END
			
			DELETE 
			FROM CW_NewCarShowroomPrices 
			WHERE isMetallic=@delColor 
			AND CityId=@CityId 
			AND CarVersionId in (SELECT ID FROM CarVersions WITH(NOLOCK) where CarModelId=@ModelId)
			
			DELETE 
			FROM NewCarShowroomPrices 
			WHERE isMetallic=@delColor 
			AND CityId=@CityId 
			AND CarVersionId in (SELECT ID FROM CarVersions WITH(NOLOCK) where CarModelId=@ModelId)
			
			
			DECLARE @CarVersionId INT
			
			SELECT TOP 1 @CarVersionId=ID FROM CarVersions WITH(NOLOCK) where CarModelId=@ModelId
			
			EXEC UpdateModelPrices @CarVersionId,@CityId
		END
	   
	  
	END

