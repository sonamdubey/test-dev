IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ReviewsHelpful]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ReviewsHelpful]
GO

	


CREATE PROCEDURE [dbo].[Acc_ReviewsHelpful]
   	@ReviewId		NUMERIC, 
	@Helpful		BIT
 AS
   
BEGIN
	IF @Helpful = 0		--disliked
		UPDATE Acc_ItemReviews
		SET	
			Disliked = IsNull(Disliked, 0) + 1
		WHERE
			ID = @ReviewId
	ELSE
		UPDATE Acc_ItemReviews
		SET	
			Liked = IsNull(Liked, 0) + 1
		WHERE
			ID = @ReviewId
    
END
