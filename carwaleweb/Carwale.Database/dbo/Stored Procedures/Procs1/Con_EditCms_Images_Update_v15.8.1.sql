IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Images_Update_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Images_Update_v15]
GO

	
-- =============================================
-- Author:		Vikas
-- Create date: 26/07/2012
-- Description:	To Update the contents of the Table
-- Modified by : Manish on 30-06-2015 increased the size of datatypes and added try and catch block
-- Modified By : Ashwini Todkar on 8 July 2015, saved small image path to Con_EditCms_Images
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_Images_Update_v15.8.1]
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
	,@ImagePath VARCHAR(150)
	,@AltImageName VARCHAR(150)
	,@Title VARCHAR(100)
	,@Description VARCHAR(300)
	,@ApplicationId TINYINT
	,@TimeStamp VARCHAR(25)
	,@IsWatermark BIT
AS
BEGIN
	BEGIN TRY
		DECLARE @PhotoGalleryId AS TINYINT

		SELECT @PhotoGalleryId = Id
		FROM Con_EditCms_Category EB WITH (NOLOCK)
		WHERE NAME = 'Photo Galleries'

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
				,ImageName = @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?v=' + @TimeStamp + '&wm=' + CONVERT(VARCHAR, @IsWatermark)
				,OriginalImgPath = @ImagePath + @ImageName + '-' + CONVERT(VARCHAR, @ImageId) + '.jpg?v=' + @TimeStamp + '&wm=' + CONVERT(VARCHAR, @IsWatermark)
			WHERE ID = @ImageId

			SELECT Id
			FROM Con_EditCms_Basic EB WITH (NOLOCK)
			WHERE Id = @BasicId
				AND CategoryId = @PhotoGalleryId
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


