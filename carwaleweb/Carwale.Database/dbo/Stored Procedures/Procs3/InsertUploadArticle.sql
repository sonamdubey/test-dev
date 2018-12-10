IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUploadArticle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUploadArticle]
GO

	




-- This procedure is used to upload articles by customer. It will effect ony table UploadedArticles

CREATE    PROCEDURE [dbo].[InsertUploadArticle]

	@CategoryId	 	NUMERIC,
	@CustomerId		NUMERIC,
	@Title			VARCHAR(100),
	@Synopsis		VARCHAR(250),
	@FileName 		VARCHAR(30),
	@Status		VARCHAR(1) 	OUTPUT,	---	@Status values  ( 0 means ERROR, 	1 means RECORD EXIST,	2 means SAVED )
	@GeneratedId		NUMERIC	OUTPUT
AS
	BEGIN
		DECLARE @RecordCount	NUMERIC
	
		SET @Status = 0
		SET @GeneratedId =	  -1
		
		INSERT UploadedArticles ( CategoryId, CustomerId, Title,  Synopsis,  FileName ) VALUES ( @CategoryId, @CustomerId, @Title, @Synopsis, @FileName )
		SET @Status = 2
		SET @GeneratedId	=	SCOPE_IDENTITY()
	END



