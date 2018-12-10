IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ReviewComments_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ReviewComments_SP]
GO

	
CREATE PROCEDURE [dbo].[Acc_ReviewComments_SP]
    	@ReviewId		NUMERIC,
	@CustomerId		NUMERIC,
	@Comments		VARCHAR(1000),
	@PostDateTime		DATETIME,
	@CommentId		NUMERIC OUTPUT
	
 AS
   
BEGIN
	INSERT INTO Acc_ReviewComments( ReviewId,  CustomerId,  Comments, PostDateTime) VALUES(@ReviewId, @CustomerId, @Comments, @PostDateTime)
 	SET @CommentId = SCOPE_IDENTITY()
END