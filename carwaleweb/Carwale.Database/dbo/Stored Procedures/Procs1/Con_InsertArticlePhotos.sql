IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertArticlePhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertArticlePhotos]
GO

	Create procedure [dbo].[Con_InsertArticlePhotos]

@ID NUMERIC,
@ARTId NUMERIC,
@Caption VARCHAR(250),
@PhotoId NUMERIC OUTPUT,
@Status VARCHAR(10) OUTPUT 

AS
	
BEGIN
	SET @Status = 'false'
	IF @ID = -1
		BEGIN
		INSERT INTO Con_ArticleAlbum 
		(ARTID,Caption) 
		VALUES 
		(@ARTId,@Caption)
		
		SET @PhotoId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_ArticleAlbum
		SET Caption=@Caption
		WHERE ID = @ID
		SET @PhotoId = @ID 
		END
	SET @Status = 'true'
END