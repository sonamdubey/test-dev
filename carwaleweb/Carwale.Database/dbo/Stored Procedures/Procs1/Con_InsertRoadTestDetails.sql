IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertRoadTestDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertRoadTestDetails]
GO

	
Create procedure [dbo].[Con_InsertRoadTestDetails]
@ID NUMERIC,
@MainImgPath VARCHAR (100),
@Caption VARCHAR(200),
@ContentPath VARCHAR(150),
@Status VARCHAR(10) OUTPUT 

AS
	
BEGIN

	SET @Status = 'false'
	UPDATE Con_RoadTestPages
	SET MainImgPath=@MainImgPath , Caption=@Caption, ContentPath=@ContentPath
	WHERE ID=@ID

	SET @Status = 'true'
END