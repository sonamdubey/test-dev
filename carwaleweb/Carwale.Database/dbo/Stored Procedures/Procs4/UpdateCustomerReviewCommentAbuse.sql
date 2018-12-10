IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerReviewCommentAbuse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerReviewCommentAbuse]
GO

	
--THIS PROCEDURE is for setting the review  comment as abuse

CREATE PROCEDURE [dbo].[UpdateCustomerReviewCommentAbuse]
	@CommentId		NUMERIC
 AS
BEGIN
	
	UPDATE CustomerReviewComments
	SET	
		ReportedAbuse = 1
	WHERE
		ID = @CommentId
			
END