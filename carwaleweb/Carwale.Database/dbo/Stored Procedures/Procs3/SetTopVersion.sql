IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SetTopVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SetTopVersion]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 28 Aug 2013
-- Description:	Sets top version for the input model
-- =============================================
CREATE PROCEDURE [dbo].[SetTopVersion] 
	-- Add the parameters for the stored procedure here
	@ModelId INT,
	@VersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	DECLARE @SubSegmentId TINYINT = (SELECT SubSegmentId FROM CarVersions WITH (NOLOCK) WHERE ID = @VersionId)
    
    UPDATE CarModels
    SET CarVersionID_Top = @VersionId, SubSegmentID = @SubSegmentId WHERE ID = @ModelId
    
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
	@UpdateType INT =13,
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
	@CarVersionID_Top int =@VersionId,
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
			@SelId,
			@CarModelId
				--mysql sync end
		end try
		BEGIN CATCH
			INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
			VALUES('MysqlSync','CON_UpdateExpectedCarLaunches_v16_9_1',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
		END CATCH			
	    SELECT Name as ss FROM CarSubSegments WITH (NOLOCK) WHERE Id = @SubSegmentId
END

