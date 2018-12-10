IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Images_SaveRQ_new_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Images_SaveRQ_new_v16]
GO

	
-- Modified By : Vinay Kumar Prajapati on 23rd Desc 2014(Model videos Thumb nail colomn added VideoPathThumbNail )
-- Modified By Satish Sharma on 3rd May 15, Synced Main images of Model Gallery to CarModels table
-- Modified By Ashwini Todkar on 10 June 2015, Synced Main images of Model Gallery to CarVersions images
-- Modified By Ashwini Todkar on 23 June 2015, append hosthurl to extra large images of Model Gallery to CarVersions,Carmodels images
-- Modified By : Ashwini Todkar on 8 July 2015, updated ImagePathSmall of image in Con_EditCms_Images
-- Modified By : Ashwini Todkar on 9 July 2015, removed model ad version photos path update query
-- Modified BY : Sanjay Soni on 19 August 2015, changes data type from bit to tinyint of IsWatermark 
-- Modified by Satish Sharma on 6th May 2016,  Save main images path and host url to basic table to avaid join at the time of select.
-- Modified by Jitendra  on 13th May 2016,  Save main images path and host url to basic table to avaid join at the time of select.
CREATE PROCEDURE [dbo].[Con_EditCms_Images_SaveRQ_new_v16.5.1] @ID INT
	,@BasicId INT
	,@ImageCategoryId INT
	,@Caption VARCHAR(250)
	,@LastUpdatedBy INT
	,@HostUrl VARCHAR(250)
	,@IsReplicated BIT
	,@ImageId INT OUT
	,@MakeId INT
	,@ModelId INT
	,@ImageName VARCHAR(300)
	,@IsMainImage BIT
	,@ImagePath VARCHAR(50)
	,@AltImageName VARCHAR(100)
	,@Title VARCHAR(100)
	,@Description VARCHAR(200)
	,@ApplicationId TINYINT
	,@IsWatermark TINYINT
	,@TimeStamp VARCHAR(25)
AS
BEGIN
	BEGIN TRY
		IF @ID = - 1
		BEGIN
			DECLARE @LastSequence AS INT

			SELECT @LastSequence = Max(Sequence)
			FROM Con_EditCms_Images WITH(NOLOCK)
			WHERE BasicId = @BasicId
				AND IsActive = 1

			SET @LastSequence = ISNULL(@LastSequence, 0)

			DECLARE @Cnt INT = 0

			IF (@IsMainImage = 1)
			BEGIN
				SELECT @Cnt = COUNT(*)
				FROM Con_EditCms_Images WITH(NOLOCK)
				WHERE BasicId = @BasicId
					AND IsMainImage = 1

				IF (@Cnt > 0)
				BEGIN
					SELECT @ImageId = ID
					FROM Con_EditCms_Images WITH(NOLOCK)
					WHERE BasicId = @BasicId
						AND IsMainImage = 1

					UPDATE Con_EditCms_Images
					SET IsMainImage = 0
					WHERE Id = @ImageId
				END
			END

			INSERT INTO Con_EditCms_Images (
				BasicId
				,ImageCategoryId
				,Caption
				,LastUpdatedTime
				,LastUpdatedBy
				,Sequence
				,HostUrl
				,MakeId
				,ModelId
				,ImageName
				,IsMainImage
				,AltImageName
				,Title
				,Description
				)
			VALUES (
				@BasicId
				,@ImageCategoryId
				,@Caption
				,GETDATE()
				,@LastUpdatedBy
				,(@LastSequence + 1)
				,@HostUrl
				,@MakeId
				,@ModelId
				,@ImageName
				,@IsMainImage
				,@AltImageName
				,@Title
				,@Description
				)

			SET @ImageId = SCOPE_IDENTITY()

			declare @_OriginalImgPath varchar(200) =  @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?wm=' + CONVERT(varchar, ISNULL(@IsWatermark, 0))

			UPDATE Con_EditCms_Images
			SET OriginalImgPath = @_OriginalImgPath
			WHERE Id = @ImageId

			IF (@IsMainImage = 1) -- Update Image to Con_EditCms_Basic to avoid join at the time of select
			BEGIN
				UPDATE Con_EditCms_Basic SET HostUrl = @HostUrl, MainImagePath = @_OriginalImgPath where Id = @BasicId
			END
		END
		ELSE
			IF @ID <> - 1
			BEGIN
				SET @ImageId = @ID

				EXEC [dbo].[Con_EditCms_Images_Update_v15.8.1] @BasicId
					,@ImageCategoryId
					,@Caption
					,@LastUpdatedBy
					,@HostUrl
					,@IsReplicated
					,@ImageId
					,@MakeId
					,@ModelId
					,@ImageName
					,@IsMainImage
					,@ImagePath
					,@AltImageName
					,@Title
					,@Description
					,@ApplicationId
					,@TimeStamp
					,@IsWatermark
					SET @ImageId = @ID 
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
			,'dbo.Con_EditCms_Images_SaveRQ_new'
			,ERROR_MESSAGE()
			,'Con_EditCms_Images'
			,@BasicId
			,GETDATE()
			)
	END CATCH;
END

