IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ItemReviews_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ItemReviews_SP]
GO

	
CREATE PROCEDURE [dbo].[Acc_ItemReviews_SP]  
	@CustomerId		NUMERIC,
	@ItemId		NUMERIC,
	@Pros			VARCHAR(100),
	@Cons			VARCHAR(100),
	@Title			VARCHAR(100),
	@Description		VARCHAR(6000),
	@EntryDateTime	DATETIME,
	@OverallRating		DECIMAL(18,2),
	@ItemReviewId		INTEGER OUTPUT

AS

	SELECT Id FROM Acc_ItemReviews WHERE ItemId = @ItemId AND CustomerId = @CustomerId
	IF @@ROWCOUNT= 0
		BEGIN
			INSERT INTO  Acc_ItemReviews(CustomerId, ItemId, Pros, Cons, Title, Description, OverallRating, EntryDateTime)
			VALUES(@CustomerId, @ItemId, @Pros, @Cons, @Title, @Description, @OverallRating, @EntryDateTime)
			
			Set @ItemReviewId = Scope_Identity()
		END
	ELSE  Set @ItemReviewId = -1
