IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateModelPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateModelPrices]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 09/07/2013
-- Description:	Update MaxPrice and MinPrice field in CarModels table
--Modified By Reshma Shetty 08/11/2013 Instead of ModelId in NewCarShowroomPrices now the modelid in CarVersions table is considered
-- Modified by Manish Chourasiya on 24-04-2014 added CarModelId condition during the update.
-- Modified by Manish Chourasiya on 24-04-2014 for updating min and max price of the models in ModelMetroCityPrices table
-- =============================================
CREATE PROCEDURE [dbo].[UpdateModelPrices] 
	-- Add the parameters for the stored procedure here
	@CarVersionId INT,
	@CityId INT=NULL   -----Parametre added by Manish on 24-04-2014
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
   -- DECLARE @CityId INT=10  
    
	DECLARE @CarModelId AS INT
	SELECT @CarModelId=CarModelId FROM CarVersions WITH (NOLOCK) WHERE ID=@CarVersionId  -----added by Manish on 24-04-2014
	IF @CityId=10 OR @CityId IS NULL
	BEGIN 
			UPDATE CMO
			SET MinPrice = CV.MinPrice ,MaxPrice=CV.MaxPrice
			FROM (	SELECT CV.CarModelId,
						MIN(Price) MinPrice,
						MAX(Price) MaxPrice
					FROM NewCarShowroomPrices NCP WITH(NOLOCK)
					INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = NCP.CarVersionId
					WHERE NCP.CityId = 10   --- Need to consider delhi price only
						  AND CV.New=1 
						  AND CV.IsDeleted=0
						  AND CV.CarModelId=@CarModelId  ---Condition added by Manish on 24-04-2014 
					GROUP BY cv.CarModelId) AS CV
			INNER JOIN CarModels CMO WITH(NOLOCK) ON CMO.ID=CV.CarModelId
			WHERE   CMO.Id=@CarModelId                   ---Condition added by Manish on 24-04-2014 
			
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
	@Id int =@CarModelId,
	@SmallPic varchar(200) =null,
	@LargePic varchar(200) =null,
	@HostUrl varchar(100) =null,
	@UpdateType INT =28,
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
	@MinPrice int =null,
	@MaxPrice int =null
		--mysql sync end
	set @UpdateType=28
	set @Id=@CarModelId
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
		VALUES('MysqlSync','UpdateModelPrices',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@ID,GETDATE(),@UpdateType)
	END CATCH
	END 
	IF (@CityId =1 OR @CityId =2 OR @CityId =10 OR @CityId =105 OR @CityId =176 OR @CityId =198 OR @CityId IS NULL)
	 BEGIN 
	  
	  DELETE FROM ModelMetroCityPrices 
	  WHERE CarModelId=@CarModelId 
	   AND (CityId=@CityId OR @CityId IS NULL)
	   AND Cityid in (1,2,10,105,176,198)
	  Insert into    ModelMetroCityPrices 
	                              (CarModelId,
								   CityId,
								   MinPrice,
								   MaxPrice,
								   CreatedOn
								  )
						  SELECT  CV.CarModelId,
								  NCP.CityId,
								  MIN(Price) MinPrice,
								  MAX(Price) MaxPrice,
								  GETDATE()
								 FROM NewCarShowroomPrices NCP WITH(NOLOCK)
								INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = NCP.CarVersionId
								INNER JOIN CarModels CM WITH(NOLOCK) ON CM.ID=CV.CarModelId
								WHERE ( NCP.CityId =@CityId OR @CityId IS NULL)
								  AND Cityid in (1,2,10,105,176,198)
								  AND CV.New=1 
								  AND CV.IsDeleted=0
								  AND CV.CarModelId=@CarModelId  
								  AND CM.New=1
								 GROUP BY cv.CarModelId,NCP.CityId
		END 
		
	
END

