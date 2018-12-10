IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_UpdateAlbum]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_UpdateAlbum]
GO

	

CREATE PROCEDURE [dbo].[UP_UpdateAlbum]
	@Id			BIGINT,
	@Name			VARCHAR(200), 
	@Description		VARCHAR(500),
	@CreationDate		DATETIME,
	@UserId		BIGINT,
	@Status		INT OUTPUT		
 AS
	
BEGIN

	SET @Status = 0

	IF @Name <> ' '
	BEGIN
		UPDATE UP_Albums SET Name = @Name
		WHERE ID = @Id
	
		SET @Status = 1 
	END
	ELSE
	BEGIN
		UPDATE UP_Albums SET Description = @Description
		WHERE ID = @Id
	
		SET @Status = 1 
	END
END
