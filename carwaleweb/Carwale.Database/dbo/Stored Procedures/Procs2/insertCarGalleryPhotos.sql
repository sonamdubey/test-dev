IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[insertCarGalleryPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[insertCarGalleryPhotos]
GO

	CREATE       PROCEDURE [dbo].[insertCarGalleryPhotos] 

@Id NUMERIC,
@CategoryId NUMERIC,
@Title VARCHAR(200),
@Description VARCHAR(1000),
@PhotoURL VARCHAR(50),
@ModelId NUMERIC,
@VersionId NUMERIC,
@EntryDate DATETIME,
@IsActive BIT,
@Status VARCHAR(10) OUTPUT, 	-- Used to show transaction completed or not
@ModelText VARCHAR(40),
@CategoryText VARCHAR(50),
@PhotoId VARCHAR(100) OUTPUT

AS
	
BEGIN
	SET @Status = 'false'
	IF @Id = -1
		BEGIN

		INSERT INTO CarGalleryPhotos 
		(CategoryId,Title,Description,PhotoURL,ModelId,VersionId,EntryDate,IsActive) 
		VALUES 
		(@CategoryId,@Title,@Description,@PhotoURL,@ModelId,@VersionId,@EntryDate,@IsActive)
	
		SET @PhotoId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE CarGalleryPhotos
		SET CategoryId = @CategoryId,
			Title = @Title, Description = @Description,
			PhotoURL = @PhotoURL, ModelId = @ModelId,
			VersionId = @VersionId, IsActive = @IsActive
		WHERE Id = @Id
		SET @PhotoId = @Id 
		END
	SET @Status = 'true'
END
