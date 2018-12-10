IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertUpdateArticle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertUpdateArticle]
GO

	
CREATE procedure [dbo].[Con_InsertUpdateArticle]
@ARTID NUMERIC,
@CatId NUMERIC,
@Title VARCHAR(250),
@DisplayDate DATETIME,
@AuthorName VARCHAR(100),
@Tags VARCHAR(500),
@PageKeywords VARCHAR(500),
@PageDesc VARCHAR(500),
@Synopsis VARCHAR(500),
@EntryDate DATETIME,
@ID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @ARTID = -1
		BEGIN
		INSERT INTO Con_Article 
		(CatId, Title, DisplayDate, AuthorName, Tags, PageKeywords, PageDesc, Synopsis, EntryDate)
		VALUES 
		(@CatId, @Title, @DisplayDate, @AuthorName, @Tags, @PageKeywords, @PageDesc, @Synopsis, @EntryDate)
		SET @ID = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_Article
		SET CatId = @CatId, Title=@Title,
		DisplayDate = @DisplayDate, AuthorName = @AuthorName, Tags = @Tags,
		PageKeywords = @PageKeywords, PageDesc = @PageDesc, Synopsis = @Synopsis
		Where ID = @ARTID
		SET @ID = @ARTID 
		END
	
END

