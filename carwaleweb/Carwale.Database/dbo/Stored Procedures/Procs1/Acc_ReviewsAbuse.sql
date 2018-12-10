IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ReviewsAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ReviewsAbuse]
GO

	


CREATE PROCEDURE [dbo].[Acc_ReviewsAbuse]
    @ReviewId		NUMERIC
 AS
   
BEGIN
	UPDATE Acc_ItemReviews
	SET	
		ReportAbuse = 1
	WHERE
		ID = @ReviewId
 
END
