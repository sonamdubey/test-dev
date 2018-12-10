IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Images_Update]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Images_Update]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 26/07/2012
-- Description:	To Update the contents of the Table
-- Modified by : Manish on 30-06-2015 increased the size of datatypes and added try and catch block
-- Modified By : Ashwini Todkar on 8 July 2015, saved small image path to Con_EditCms_Images
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_Images_Update]
	-- Add the parameters for the stored procedure here	
	@BasicId NUMERIC(18, 0)
	,@ImageCategoryId NUMERIC(18, 0)
	,@Caption VARCHAR(250)
	,@LastUpdatedBy NUMERIC(18, 0)
	,@HostUrl VARCHAR(250)
	,@IsReplicated BIT
	,@ImageId NUMERIC(18, 0)
	,@MakeId INT
	,@ModelId INT
	,@ImageName VARCHAR(150)
	,@IsMainImage BIT
	,@HasCustomImage BIT
	,@ImagePath VARCHAR(150)
	,@AltImageName VARCHAR(150)
	,@Title VARCHAR(100)
	,@Description VARCHAR(300)
	,@ApplicationId TINYINT
	,@TimeStamp VARCHAR(25)
	,@IsHomePageImg BIT
AS
BEGIN
	BEGIN TRY
		DECLARE @PhotoGalleryId AS TINYINT
		SELECT @PhotoGalleryId = Id
		FROM Con_EditCms_Category EB WITH (NOLOCK)
		WHERE NAME = 'Photo Galleries'
		IF (@ApplicationId = 1)
		BEGIN
			UPDATE Con_EditCms_Images
			SET ImageCategoryId = @ImageCategoryId
				,Caption = @Caption
				,AltImageName = @AltImageName
				,Title = @Title
				,Description = @Description
				,LastUpdatedTime = GETDATE()
				,LastUpdatedBy = @LastUpdatedBy
				,HostUrl = @HostUrl
				,IsReplicated = @IsReplicated
				,MakeId = @MakeId
				,ModelId = @ModelId
				,ImageName = @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
				,IsMainImage = @IsMainImage
				,HasCustomImg = @HasCustomImage
				,ImagePathThumbnail = (
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_m.jpg?' + @TimeStamp
						ELSE @ImagePath + 't/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
						END
					)
				,ImagePathLarge = (
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_l.jpg?' + @TimeStamp
						ELSE @ImagePath + 'l/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
						END
					)
				,ImagePathOriginal = (
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_ol.jpg?' + @TimeStamp
						ELSE @ImagePath + 'ol/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
						END
					)
				,ImagePathCustom = (
					CASE 
						WHEN HasCustomImg = 1
							THEN CASE 
									WHEN IsMainImage = 1
										THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c.jpg?' + @TimeStamp
									ELSE @ImagePath + 'c/' + @ImageName + '?' + @TimeStamp
									END
						ELSE NULL
						END
					)
					,ImagePathSmall = (
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_s.jpg?' + @TimeStamp
						END
					) -- Added By Ashwini Todkar on 8 July 2015, saved small image path
			WHERE ID = @ImageId
			SELECT Id
			FROM Con_EditCms_Basic EB WITH (NOLOCK)
			WHERE Id = @BasicId
				AND CategoryId = @PhotoGalleryId
			IF @@ROWCOUNT > 0
			BEGIN
				IF @ModelId > 0
					AND @IsMainImage = 1
				BEGIN
					UPDATE CarModels
					SET XLargePic = @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_l.jpg?' + @TimeStamp
					WHERE CarModels.ID = @ModelId
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
		
		set @XLargePic= @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_l.jpg?' + @TimeStamp
		set @Id= @ModelId
		set @UpdateType=18
		 
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
		VALUES('MysqlSync','Con_EditCms_Images_Update',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
				END
			END
		END
		ELSE
			IF (@ApplicationId = 2)
			BEGIN
				UPDATE Con_EditCms_Images
				SET ImageCategoryId = @ImageCategoryId
					,Caption = @Caption
					,AltImageName = @AltImageName
					,Title = @Title
					,Description = @Description
					,LastUpdatedTime = GETDATE()
					,LastUpdatedBy = @LastUpdatedBy
					,HostUrl = @HostUrl
					,IsReplicated = @IsReplicated
					,MakeId = @MakeId
					,ModelId = @ModelId
					,ImageName = @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
					,IsMainImage = @IsMainImage
					,HasCustomImg = @HasCustomImage
					,ImagePathThumbnail = (
						CASE 
							WHEN IsMainImage = 1
								THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_m.jpg?' + @TimeStamp
							ELSE @ImagePath + 't/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
							END
						)
					,ImagePathLarge = (
						CASE 
							WHEN IsMainImage = 1
								THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_l.jpg?' + @TimeStamp
							ELSE @ImagePath + 'l/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
							END
						)
					,ImagePathOriginal = (
						CASE 
							WHEN IsMainImage = 1
								THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_ol.jpg?' + @TimeStamp
							ELSE @ImagePath + 'ol/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
							END
						)
					,ImagePathCustom = (
						CASE 
							WHEN HasCustomImg = 1
								THEN CASE 
										WHEN IsMainImage = 1
											THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c.jpg?' + @TimeStamp
										ELSE @ImagePath + 'c/' + @ImageName + @TimeStamp
										END
							ELSE NULL
							END
						)
					,ImagePathCustom200 = (
						CASE 
							WHEN HasCustomImg = 1
								AND @IsHomePageImg = 1
								THEN CASE 
										WHEN IsMainImage = 1
											THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c200.jpg?' + @TimeStamp
										ELSE @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c200.jpg?' + @TimeStamp
										END
							ELSE NULL
							END
						)
					,ImagePathCustom140 = (
						CASE 
							WHEN HasCustomImg = 1
								AND @IsHomePageImg = 1
								THEN CASE 
										WHEN IsMainImage = 1
											THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c140.jpg?' + @TimeStamp
										ELSE @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c140.jpg?' + @TimeStamp
										END
							ELSE NULL
							END
						)
					,ImagePathCustom88 = (
						CASE 
							WHEN HasCustomImg = 1
								AND @IsHomePageImg = 1
								THEN CASE 
										WHEN IsMainImage = 1
											THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c88.jpg?' + @TimeStamp
										ELSE @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c88.jpg?' + @TimeStamp
										END
							ELSE NULL
							END
						)
				WHERE ID = @ImageId
			END
	END TRY
	BEGIN CATCH
		INSERT INTO CarWaleWebSiteExceptions (
			ModuleName
			,SPName
			,ErrorMsg
			,TableName
			,FailedId
			,CreatedOn
			)
		VALUES (
			'Image Replication from RabbitMQ'
			,'dbo.Con_EditCms_Images_Update'
			,ERROR_MESSAGE()
			,'Con_EditCms_Images'
			,@BasicId
			,GETDATE()
			)
	END CATCH
END

