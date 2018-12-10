IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertAlbum]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertAlbum]
GO

	
CREATE PROCEDURE [dbo].[UP_InsertAlbum]
	@Name			VARCHAR(200), 
	@Description		VARCHAR(500),
	@CreationDate		DATETIME,
	@UserId		BIGINT,
	@AlbumId		NUMERIC OUTPUT
AS
	
BEGIN

	INSERT INTO UP_Albums(Name, Description, CreationDate,Photos, UserId,IsActive ) 
	VALUES(@Name, @Description, @CreationDate,0, @UserId, 1)

	SET @AlbumId = SCOPE_IDENTITY() 

END
