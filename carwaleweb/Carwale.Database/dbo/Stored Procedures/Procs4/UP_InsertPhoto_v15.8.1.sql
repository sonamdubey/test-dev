IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertPhoto_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertPhoto_v15]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[UP_InsertPhoto_v15.8.1]

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
@ImageUrlOriginal  VARCHAR(200)


	SET @Status = 0

	IF @Id = -1

		BEGIN

			INSERT INTO UP_Photos(Title, Name, Description, EntryDate, AlbumId, Views, Rating, Size500, Size800, Size1024, IsActive, MarkAbuse, HostURL ,DirectoryPath) 
			VALUES(@Title, @Name, @Description, @EntryDate, @AlbumId, 0, 0,@Size500, @Size800, @Size1024, 1, 0, @HostUrl ,@DirectoryPath)
			
			SET @PhotoId = Scope_Identity()	
			SET @ImageUrlOriginal = @Name+'_O.jpg'
			
			Update UP_Photos SET OriginalImgPath= @DirectoryPath+@ImageUrlOriginal
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

