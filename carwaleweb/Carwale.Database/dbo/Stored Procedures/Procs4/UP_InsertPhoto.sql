IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertPhoto]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[UP_InsertPhoto]

	@Id			BIGINT,
	@Title			VARCHAR(200), 
	@Name			VARCHAR(200), 
	@Description		VARCHAR(500),
	@EntryDate		DATETIME,
	@AlbumId		BIGINT,
	@PhotoId		BIGINT OUTPUT,
	@Size500		BIT,
	@Size800		BIT,
	@Size1024		BIT,
	@Status		INT OUTPUT,
	@HostUrl	VARCHAR(100) = NULL,
	@DirectoryPath varchar(100)
 AS
	DECLARE @TotalPhotos NUMERIC

BEGIN

DECLARE 
@ImageUrlSmall  VARCHAR(200),
@ImageUrlThumbnail  VARCHAR(200),
@ImageUrlMedium VARCHAR(200),
@ImageUrlLarge VARCHAR(200),
@ImageUrlXL VARCHAR(200),
@ImageUrlXXL VARCHAR(200)

	SET @Status = 0

	IF @Id = -1

		BEGIN

			INSERT INTO UP_Photos(Title, Name, Description, EntryDate, AlbumId, Views, Rating, Size500, Size800, Size1024, IsActive, MarkAbuse, HostURL ,DirectoryPath) 
			VALUES(@Title, @Name, @Description, @EntryDate, @AlbumId, 0, 0,@Size500, @Size800, @Size1024, 1, 0, @HostUrl ,@DirectoryPath)
			
			SET @PhotoId = Scope_Identity()	
			SET @ImageUrlSmall = @Name+'_75.jpg'
			SET @ImageUrlThumbnail = @Name+'_100.jpg'
			SET @ImageUrlMedium = @Name+'_160.jpg'
			SET @ImageUrlLarge = @Name+'_500.jpg'
			SET @ImageUrlXL = @Name+'_800.jpg'
			SET @ImageUrlXXL = @Name+'_1024.jpg'
			Update UP_Photos SET Small = @ImageUrlSmall , Thumbnail = @ImageUrlThumbnail, Medium = @ImageUrlMedium , 
			Large = @ImageUrlLarge, XL = @ImageUrlXL , XXL = @ImageUrlXXL
			WHERE ID = @PhotoId

			SELECT @TotalPhotos=Photos FROM UP_Albums WHERE ID = @AlbumId
			
			UPDATE UP_Albums SET Photos =  @TotalPhotos + 1 WHERE  ID = @AlbumId



			SET @Status = 1 
			
			
		END
	ELSE
		
		BEGIN
			UPDATE UP_Photos SET Title=@Title, Name = @Name, Description = @Description
			WHERE ID = @Id

			SET @Status = 1 
		END
END


