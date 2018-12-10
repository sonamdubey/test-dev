IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerReviewsHelpful]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerReviewsHelpful]
GO

	
--THIS PROCEDURE is for updating the count of the customer reviews helpful and the disliked field

CREATE PROCEDURE [dbo].[UpdateCustomerReviewsHelpful]
	@ReviewId		NUMERIC, 
	@Helpful		BIT
 AS
	--DECLARE
		--@TempCount	AS NUMERIC
BEGIN
	
	IF @Helpful = 0		--disliked
		UPDATE CustomerReviews
		SET	
			Disliked = IsNull(Disliked, 0) + 1
		WHERE
			ID = @ReviewId
	ELSE
		UPDATE CustomerReviews
		SET	
			Liked = IsNull(Liked, 0) + 1
		WHERE
			ID = @ReviewId
			
END
