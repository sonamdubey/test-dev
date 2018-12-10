IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertMediaPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertMediaPhotos]
GO

	CREATE procedure [dbo].[Con_InsertMediaPhotos]

@ID NUMERIC,
@Name VARCHAR(100),
@PhotoId NUMERIC OUTPUT,
@Status BIT OUTPUT 

AS
	
BEGIN
	SET @Status = 0
	IF @ID = -1
		BEGIN
		INSERT INTO Media_Publisher 
		(Name) 
		VALUES 
		(@Name)
		
		SET @PhotoId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Media_Publisher
		SET Name=@Name
		WHERE ID = @ID
		SET @PhotoId = @ID 
		END
	SET @Status = 1
END