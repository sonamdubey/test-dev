IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_UpdateAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_UpdateAbuse]
GO

	
--THIS PROCEDURE is for setting the review comment as abuse

CREATE PROCEDURE [dbo].[UP_UpdateAbuse]
	@PhotoId		NUMERIC
 AS
BEGIN
	
	UPDATE UP_Photos
	SET	
		MarkAbuse = 1
	WHERE
		ID = @PhotoId
			
END
