IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GenModelSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GenModelSummary]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 01-07-2013
-- Description:	
-- =============================================
/*
	14	Displacement
	15	Max Power
	26	Fuel Type
	29	Transmission Type
	30	No of gears
	exec [CD].[GenModelSummary] 2282,14
*/
-- Modified by Avishkar 5-8-2014 to use with (nolock) 
CREATE PROCEDURE [CD].[GenModelSummary]
	-- Add the parameters for the stored procedure here
	@VersionID INT,
	@ItemMasterID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Items TABLE
	(
		ID INT IDENTITY,
		ItemMasterID INT
	)
	
	INSERT INTO @Items (ItemMasterID) VALUES (14),(15),(26),(29),(30)
	IF EXISTS (SELECT * FROM @Items WHERE ItemMasterID = @ItemMasterID)
		BEGIN
		DECLARE @ModelID INT	
		DECLARE @Segement VARCHAR(20)
		DECLARE @BodyStyle VARCHAR(20)
		DECLARE @Displacement VARCHAR(100) = ''
		DECLARE @Power VARCHAR(100) = ''
		DECLARE @Fuel VARCHAR(100) = ''
		DECLARE @NoOfGears VARCHAR(100) = ''
		DECLARE @Transmission VARCHAR(100) = ''
		DECLARE @Summary NVARCHAR(MAX) = ''
		
		-- Modified by Avishkar 5-8-2014 to use WITH (NOLOCK) 
		SELECT @ModelID = CarModelId, @Segement = CS.Name, @BodyStyle = CB.Name FROM CarVersions CV WITH (NOLOCK) 
		LEFT JOIN CarSegments CS WITH (NOLOCK)  ON CV.SegmentId = CS.ID
		LEFT JOIN CarBodyStyles CB WITH (NOLOCK)  ON CV.BodyStyleId = CB.ID
		WHERE CV.ID = @VersionID
				
		SET @Summary = @BodyStyle +', '+ @Segement
		
		DECLARE @Versions TABLE
		(
			ID INT IDENTITY,
			VersionID INT
		)
		--select @ModelID
		INSERT INTO @Versions 
		SELECT CV.ID FROM CarVersions CV WITH (NOLOCK) 
		WHERE CV.CarModelId = @ModelID  AND New = 1
		--select * from @Versions
		--SELECT * FROM CD.ItemValues WHERE CarVersionId IN (SELECT ID FROM CarVersions WHERE CarModelId = @ModelID)
		
		--SELECT @ModelID,@Segement,@BodyStyle	
		
		DECLARE @Values TABLE
		(
			ID INT IDENTITY,
			ItemMasterID INT,
			Value VARCHAR(50)
		)
		
		INSERT INTO @Values
		SELECT DISTINCT IV.ItemMasterId, COALESCE(CAST(IV.ItemValue AS VARCHAR(20)),UD.Name,IV.CustomText) Value
		FROM CD.ItemValues IV WITH (NOLOCK) 
		LEFT JOIN CD.UserDefinedMaster UD WITH (NOLOCK)  ON IV.UserDefinedId = UD.UserDefinedId
		WHERE CarVersionId IN (SELECT VersionID FROM @Versions)
		AND IV.ItemMasterId IN (SELECT ItemMasterId FROM @Items)
		ORDER BY Value ASC
		
		--select * from @Values
		IF((SELECT COUNT(ID) FROM @Values) > 0)
		BEGIN
			
			SELECT @Displacement = @Displacement + Value + '/' FROM @Values WHERE ItemMasterID = 14
			SELECT @Power = @Power + Value + '/' FROM @Values WHERE ItemMasterID = 15
			SELECT @Fuel = @Fuel + Value + '/' FROM @Values WHERE ItemMasterID = 26
			SELECT @NoOfGears = @NoOfGears + Value + ',' FROM @Values WHERE ItemMasterID = 30
			SELECT @Transmission = @Transmission + Value + '/' FROM @Values WHERE ItemMasterID = 29		
			
			--SELECT @Displacement,@Power,@Fuel,@NoOfGears,@Transmission
			
			IF(@Displacement != '')
			BEGIN
				SET @Summary += ', ' + LEFT(@Displacement, LEN(@Displacement) - 1) +' cc'
			END
			IF(@Power != '')
			BEGIN
				SET @Summary += ', ' + LEFT(@Power, LEN(@Power) - 1) + ' bhp'
			END
			IF(@Fuel != '')
			BEGIN
				SET @Summary += ', ' + LEFT(@Fuel, LEN(@Fuel) - 1)
			END
			IF(@NoOfGears != '')
			BEGIN
				SET @Summary += ', ' + LEFT(@NoOfGears, LEN(@NoOfGears) - 1) +' Speed '
			END
			IF(@Transmission != '' AND @NoOfGears != '')
			BEGIN
				SET @Summary += '('+ LEFT(@Transmission, LEN(@Transmission) - 1) +')'
			END
			ELSE IF(@Transmission != '' AND @NoOfGears = '')
			BEGIN
				SET @Summary += ', ' + LEFT(@Transmission, LEN(@Transmission) - 1) +' Transmission'
			END
			--SELECT @Transmission
			--SELECT @Summary
		END
		SELECT @Summary
		UPDATE CarModels SET Summary = @Summary WHERE ID = @ModelID
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
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null
	
	set @Id = @ModelID
	set @UpdateType=17
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
		VALUES('MysqlSync','GenModelSummary',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH		
	--mysql sync end
		PRINT 'UPDATED'
	END
END

