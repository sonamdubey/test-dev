IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PostCommentOnUserReview]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PostCommentOnUserReview]
GO

	--THIS PROCEDURE IS FOR entry of customer reviews
--CustomerReviews
--CustomerId, MakeId, ModelId, VersionId, StyleR, ComfortR, PerformanceR, ValueR, FuelEconomyR, OverallR, Pros, Cons, Comments, Title, 
--EntryDateTime, IsNew, ReportAbused, Liked, Disliked

CREATE PROCEDURE [dbo].[PostCommentOnUserReview]
	@ReviewId		NUMERIC, 
	@CustomerId		NUMERIC, 
	@Comments		VARCHAR(500),
	@PostDateTime		DATETIME,
	@IsApproved		BIT,
	@CommentId		NUMERIC OUTPUT
	
 AS
	
BEGIN
	
	--IT IS FOR THE INSERT
	
	INSERT INTO CustomerReviewComments(CustomerId, ReviewId, Comments, PostDateTime, IsApproved)
	VALUES( @CustomerId, @ReviewId, @Comments, @PostDateTime, @IsApproved)
	
	SET @CommentId = SCOPE_IDENTITY()	
END