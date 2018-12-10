IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ItemReviewDetails_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ItemReviewDetails_SP]
GO

	
CREATE PROCEDURE [dbo].[Acc_ItemReviewDetails_SP] 
	@ItemReviewId	 	NUMERIC,
	@RatingParameterId	NUMERIC,
	@RatingValue		Decimal(18,2),
	@Status		INTEGER  OUTPUT
AS
	SELECT ItemReviewId FROM Acc_ItemReviewDetails WHERE ItemReviewId = @ItemReviewId AND RatingParameterId = @RatingParameterId
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO  Acc_ItemReviewDetails(ItemReviewId, RatingParameterId, RatingValue)
			VALUES(@ItemReviewId, @RatingParameterId, @RatingValue)
			
			Set @Status = 1
		END
	ELSE  Set @Status = -1
