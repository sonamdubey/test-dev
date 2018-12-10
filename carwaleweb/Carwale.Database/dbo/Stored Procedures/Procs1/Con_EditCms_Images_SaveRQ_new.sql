IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Images_SaveRQ_new]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Images_SaveRQ_new]
GO

	-- Modified By : Vinay Kumar Prajapati on 23rd Desc 2014(Model videos Thumb nail colomn added VideoPathThumbNail )
-- Modified By Satish Sharma on 3rd May 15, Synced Main images of Model Gallery to CarModels table
-- Modified By Ashwini Todkar on 10 June 2015, Synced Main images of Model Gallery to CarVersions images
-- Modified By Ashwini Todkar on 23 June 2015, append hosthurl to extra large images of Model Gallery to CarVersions,Carmodels images
-- Modified By : Ashwini Todkar on 8 July 2015, updated ImagePathSmall of image in Con_EditCms_Images
-- Modified By : Ashwini Todkar on 9 July 2015, removed model ad version photos path update query
CREATE PROCEDURE [dbo].[Con_EditCms_Images_SaveRQ_new] @ID INT
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
	,@HasCustomImage BIT
	,@ImagePath VARCHAR(50)
	,@AltImageName VARCHAR(100)
	,@Title VARCHAR(100)
	,@Description VARCHAR(200)
	,@ApplicationId TINYINT
	,@TimeStamp VARCHAR(25)
	,@IsHomePageImg BIT
AS
BEGIN
    BEGIN TRY

	IF @ID = - 1
	BEGIN
		DECLARE @LastSequence AS INT

		SELECT @LastSequence = Max(Sequence)
		FROM Con_EditCms_Images WITH (NOLOCK)
		WHERE BasicId = @BasicId
			AND IsActive = 1

		SET @LastSequence = ISNULL(@LastSequence, 0)

		DECLARE @Cnt INT = 0

		IF (@IsMainImage = 1)
		BEGIN
			SELECT @Cnt = COUNT(*)
			FROM Con_EditCms_Images WITH (NOLOCK)
			WHERE BasicId = @BasicId
				AND IsMainImage = 1

			IF (@Cnt > 0)
			BEGIN
				SELECT @ImageId = ID
				FROM Con_EditCms_Images WITH (NOLOCK)
				WHERE BasicId = @BasicId
					AND IsMainImage = 1

				--Exec Con_EditCms_Images_Update @BasicId, @ImageCategoryId, @Caption, @LastUpdatedBy, @HostUrl, 
				--								 @IsReplicated, @ImageId, @MakeId, @ModelId, @ImageName, @IsMainImage, @HasCustomImage, @ImagePath 
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
			,IsMainImage
			,HasCustomImg
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
			,@IsMainImage
			,@HasCustomImage
			,@AltImageName
			,@Title
			,@Description
			)

		SET @ImageId = SCOPE_IDENTITY()

		IF (@ApplicationId = 1)
		BEGIN
			UPDATE Con_EditCms_Images
			SET ImageName = @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
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
				,VideoPathThumbNail = (
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_vt.jpg?' + @TimeStamp
						ELSE @ImagePath + 'vt/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
						END
					)
				,ImagePathCustom = (
					CASE 
						WHEN HasCustomImg = 1
							THEN CASE 
									WHEN IsMainImage = 1
										THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c.jpg?' + @TimeStamp
									ELSE @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
									END
						ELSE NULL
						END
					)
					,ImagePathSmall = ( 
					CASE 
						WHEN IsMainImage = 1
							THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_s.jpg?' + @TimeStamp
						END
					)--Added By Ashwini Todkar on 8 July 2015, saved small image path
			WHERE Id = @ImageId
			--Commented by : Ashwini Todkar on 9 July 2015 as this logic moved to IMG_CarPhotosUpdateReplicateRQ 
			/*SELECT Id
			FROM Con_EditCms_Basic EB WITH (NOLOCK)
			WHERE Id = @BasicId
				AND CategoryId = 10 --Photo Galleries

			IF @@ROWCOUNT > 0
			BEGIN
				IF @ModelId > 0
					AND @IsMainImage = 1
				BEGIN
					UPDATE CM
					SET CM.XLargePic = CIM.HostURL + CIM.ImagePathLarge --append host url for extra large image path by Ashwini on 23 June 2015
						,CM.SmallPic = CIM.ImagePathSmall --Added By Ashwini Todkar on 8 July 2015, saved small image path to carmodels
						,CM.LargePic = CIM.ImagePathCustom
						,CM.HostURL = CIM.HostURL
					FROM CarModels CM WITH (NOLOCK)
					JOIN Con_EditCms_Images CIM WITH (NOLOCK) ON CM.ID = CIM.ModelId
					WHERE CIM.Id = @ImageId

					-- Write SQL query to update version image for non special versions
					-- Added by Ashwini Todkar to Update version images same as model image
					UPDATE CV
					SET CV.SmallPic = CIM.ImagePathSmall  --Added By Ashwini Todkar on 8 July 2015, saved small image path to carversions
						,CV.LargePic = CIM.ImagePathCustom
						,CV.HostURL = CIM.HostURL
					FROM CarVersions CV WITH (NOLOCK)
					JOIN Con_EditCms_Images CIM WITH (NOLOCK) ON CV.CarModelId = CIM.ModelId
					WHERE CIM.Id = @ImageId
						AND CV.SpecialVersion = 0 --Special version having different image than model image
				END
			END*/
		END
		ELSE
			IF (@ApplicationId = 2)
			BEGIN
				UPDATE Con_EditCms_Images
				SET ImageName = @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
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
					,VideoPathThumbNail = (
						CASE 
							WHEN IsMainImage = 1
								THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_vt.jpg?' + @TimeStamp
							ELSE @ImagePath + 'vt/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
							END
						)
					,ImagePathCustom = (
						CASE 
							WHEN HasCustomImg = 1
								THEN CASE 
										WHEN IsMainImage = 1
											THEN @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '_c.jpg?' + @TimeStamp
										ELSE @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?' + @TimeStamp
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
				WHERE Id = @ImageId
			END
	END
	ELSE
		IF @ID <> - 1
		BEGIN
			SET @ImageId = @ID

			EXEC Con_EditCms_Images_Update @BasicId
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
				,@HasCustomImage
				,@ImagePath
				,@AltImageName
				,@Title
				,@Description
				,@ApplicationId
				,@TimeStamp
				,@IsHomePageImg
				--SET @ImageId = @ID 
		END
		END TRY
		BEGIN CATCH
		INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Image Replication from RabbitMQ',
									        'dbo.Con_EditCms_Images_SaveRQ_new',
											 ERROR_MESSAGE(),
											 'Con_EditCms_Images',
											 @BasicId,
											 GETDATE()
                                            )


		END CATCH;

END
/****** Object:  StoredProcedure [dbo].[IMG_CarPhotosUpdateReplicateRQ]    Script Date: 07/09/2015 16:04:13 ******/
SET ANSI_NULLS ON
