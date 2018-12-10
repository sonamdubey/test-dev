IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AP_GetModelTopVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AP_GetModelTopVersions]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <24/3/2014>
-- Description:	<Get model topversion ,update carmodel and find similar models>
-- =============================================
CREATE PROCEDURE [dbo].[Con_AP_GetModelTopVersions]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	DECLARE @ROWCNT			INT
	DECLARE @ROWNO			INT
	DECLARE @ModelId		SMALLINT
	DECLARE @ModelBodyStyle	INT
	DECLARE @SubSegmentId	INT
	DECLARE @TempModel Table(RowID INT IDENTITY(1, 1),ModelID BIGINT,ModelBodyStyle INT,SubSegmentID INT)
	--UPDATE CARMODELS WHEN TOPVERSION FOR MODEL CHANGED
	UPDATE MO SET MO.CarVersionID_Top = VC.VersionId , MO.SubSegmentID = CV.SubSegmentID , MO.ModelBodyStyle=CV.BodyStyleId
	FROM CarModels AS MO
	INNER JOIN TopVersionCar VC WITH(NOLOCK) ON VC.Modelid = MO.ID
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.Id = VC.VersionId
	
	--Prasad commented following. For syncing, [Con_VerifySimilarVersion] will return a table which will be passed to Mysql via C# code
	--	--mysql sync
	--declare
	--@Name varchar(30) =null,
	--@CarMakeId numeric(18, 0) =null,
	--@IsDeleted bit =null,
	--@MoCreatedOn datetime =null,
	--@MoUpdatedOn datetime =null,
	--@MoUpdatedBy numeric(18, 0) =null,
	--@Used bit =null,
	--@New bit =null,
	--@Indian bit =null,
	--@Imported bit =null,
	--@Classic bit =null,
	--@Futuristic bit =null,
	--@Modified bit =null,
	--@Id int =null,
	--@SmallPic varchar(200) =null,
	--@LargePic varchar(200) =null,
	--@HostUrl varchar(100) =null,
	--@UpdateType INT =7,
	--@DiscontinuitionId int =null,
	--@ReplacedByModelId smallint =null,
	--@comment Varchar(max) = null,
	--@Discontinuition_date datetime = null,
	--@Maskingname varchar(50) = null,
	--@RootId smallint =null,
	--@Platform varchar(50)=null, 
	--@Generation tinyint=null, 
	--@Upgrade tinyint =null, 
	--@ModelLaunchDate datetime =null,
	--@CarVersionID_Top int =null,
	--@IsSolidColor bit =null,
	--@IsMetallicColor bit =null,
	--@MinPrice int =null, 
	--@MaxPrice int =null,
	--@ReviewRate decimal = null,   
	--@Looks decimal = null,   
	--@Comfort decimal = null,   
	--@Performance decimal = null,   
	--@ValueForMoney decimal = null,   
	--@FuelEconomy decimal = null,   
	--@ReviewCount decimal = null,
	--@Summary varchar(max) = null,
	--@OriginalImgPath varchar(150)= null,
	--@XLargePic varchar(150)=null,
	--@CV_ID int = null,
	--@SelId varchar(8000)=null
	--	--mysql sync end
		
	--	--mysql sync
	--	exec [dbo].[SyncCarModelsWithMysqlUpdate]	
	--@Name ,
	--@CarMakeId,
	--@IsDeleted,
	--@MoCreatedOn,
	--@MoUpdatedOn,
	--@MoUpdatedBy,
	--@Used ,
	--@New ,
	--@Indian ,
	--@Imported ,
	--@Classic ,
	--@Futuristic ,
	--@Modified ,
	--@Id,
	--@SmallPic ,
	--@LargePic ,
	--@HostUrl ,
	--@UpdateType ,
	--@DiscontinuitionId  ,
	--@ReplacedByModelId  ,
	--@comment,
	--@Discontinuition_date ,
	--@Maskingname ,
	--@RootId  ,
	--@Platform , 
	--@Generation , 
	--@Upgrade  , 
	--@ModelLaunchDate  ,
	--@SubSegmentId ,
	--@CarVersionID_Top  ,
	--@IsSolidColor  ,
	--@IsMetallicColor  ,
	--@MinPrice  , 
	--@MaxPrice  ,
	--@ReviewRate ,   
	--@Looks ,   
	--@Comfort ,   
	--@Performance ,   
	--@ValueForMoney ,   
	--@FuelEconomy ,   
	--@ReviewCount ,
	--@Summary,
	--@OriginalImgPath ,
	--@XLargePic ,
	--@CV_ID,
	--@SelId
	--	--mysql sync end
	--TEMPORARY TABLE HAVING ALL ACTIVE AND NEW MODELS
	INSERT INTO @TempModel
	SELECT ID,ModelBodyStyle,SubSegmentID FROM CarModels WHERE New = 1 AND IsDeleted = 0
	SET @ROWCNT = @@ROWCOUNT
	SET @ROWNO = 1
	WHILE(@ROWNO <= @ROWCNT)
	BEGIN
		SELECT @ModelId = ModelID , @ModelBodyStyle=ModelBodyStyle , @SubSegmentId= SubSegmentID 
		FROM @TempModel WHERE RowID = @ROWNO
		--FIND SIMILAR CARS WHEN TOPVERSION CHANGED
		EXEC Con_InsertSimilarCars @ModelId,@ModelBodyStyle,@SubSegmentId
		SET @ROWNO = @ROWNO + 1 
	END
END

