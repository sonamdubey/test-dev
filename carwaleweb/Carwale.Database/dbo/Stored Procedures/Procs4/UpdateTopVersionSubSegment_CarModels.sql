IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTopVersionSubSegment_CarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTopVersionSubSegment_CarModels]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 07-Aug-2013
-- Description:	Update CarVersion_Top and SubSegment column in CarModels
-- Modification by Amit Verma: Added @CarModelId input parameter to update a specific model
-- Modification by Amit Verma 28 Aug 2013: Added a select statement when @CarModelId is not null 
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTopVersionSubSegment_CarModels]
	-- Add the parameters for the stored procedure here
	@CarModelId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	WITH CTE AS(
	SELECT * FROM (
	SELECT CarModelId,ID,SubSegmentId,CNT,Price,ROW_NUMBER()OVER(PARTITION BY CarModelId ORDER BY CNT DESC,Price DESC) MCNT FROM(
	SELECT CM.ID CarModelId,CV.ID,CV.SubSegmentId,Price,COUNT(NCP.Id)CNT
	FROM CarModels CM WITH (NOLOCK)
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON CM.ID = CV.CarModelId AND CV.IsDeleted = 0 AND CV.New = 1 
	LEFT JOIN NewCarShowroomPrices NCS WITH (NOLOCK) ON NCS.CarVersionId=CV.ID AND CityId=10
	LEFT JOIN NewCarPurchaseInquiries NCP WITH (NOLOCK) ON CV.ID = NCP.CarVersionId
	WHERE CM.New = 1 AND CM.IsDeleted = 0 AND (CM.ID = @CarModelId OR @CarModelId IS NULL)
	GROUP BY CM.ID,CV.ID,Price,CV.SubSegmentId)AS Tab)AS Tab2 WHERE MCNT=1)
	UPDATE CarModels
	SET CarVersionID_Top = CTE.ID, SubsegmentId = CTE.SubSegmentId
	FROM CTE
	WHERE CarModelId=CarModels.ID
	
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
	@UpdateType INT =26,
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
	@SelId varchar(8000)=null
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
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateRecentFromUpcomingLaunches',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',-1,GETDATE(),@UpdateType)
	END CATCH
	--mysql sync end
	DECLARE @VersionId INT
	IF(@CarModelId IS NOT NULL) --Modification by Amit Verma 28 Aug 2013
	BEGIN
		SELECT CM.CarVersionID_Top vid, CV.Name 'ver',CSS.Name 'ss' FROM CarModels CM
		LEFT JOIN CarVersions CV ON CM.CarVersionID_Top = CV.ID
		LEFT JOIN CarSubSegments CSS ON CM.SubSegmentID = CSS.Id
		WHERE CM.ID = @CarModelId
		
		SELECT @VersionId=CM.CarVersionID_Top  FROM CarModels CM
		LEFT JOIN CarVersions CV ON CM.CarVersionID_Top = CV.ID
		LEFT JOIN CarSubSegments CSS ON CM.SubSegmentID = CSS.Id
		WHERE CM.ID = @CarModelId
	END	
	
	 -- Avishkar 07-08-2014 Added for logging
    INSERT INTO ModelTopVersionLogs(SPName,CarModelId,carVersionId) VALUES('UpdateTopVersionSubSegment_CarModels',@CarModelId,@VersionId)
END

