IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertRoadTestPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertRoadTestPhotos]
GO

	CREATE Procedure [dbo].[Con_InsertRoadTestPhotos]

@ID NUMERIC,
@RTId NUMERIC,
@Caption VARCHAR(250),
@CatId NUMERIC,
@PhotoId NUMERIC OUTPUT,
@Status VARCHAR(10) OUTPUT 

AS
	
BEGIN
	SET @Status = 'false'
	IF @ID = -1
		BEGIN
		INSERT INTO Con_RoadTestAlbum 
		(RTId,Caption,CategoryId) 
		VALUES 
		(@RTId,@Caption,@CatId)
		
		SET @PhotoId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_RoadTestAlbum
		SET Caption=@Caption,
		CategoryId=@CatId
		WHERE ID = @ID
		SET @PhotoId = @ID 
		END
	SET @Status = 'true'
END



