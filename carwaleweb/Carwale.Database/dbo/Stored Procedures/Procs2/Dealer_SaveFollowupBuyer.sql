IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Dealer_SaveFollowupBuyer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Dealer_SaveFollowupBuyer]
GO

	

CREATE PROCEDURE [dbo].[Dealer_SaveFollowupBuyer]
	@InquiryId		NUMERIC,
	@CustomerId		NUMERIC,
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC,
	@Comment		VARCHAR(150),
	@NextCallDate	DATETIME,
	@IsCallFinished	BIT
 AS
	
BEGIN
	
	UPDATE Dealer_BuyerFollowup 
	SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, 
		NextCallDate = @NextCallDate, Comment = Comment + '<BR>' + @Comment,
		IsCallFinished = @IsCallFinished
	WHERE InquiryId = @InquiryId
	
	IF @@ROWCOUNT = 0
		BEGIN
			
			INSERT INTO Dealer_BuyerFollowup
			(
				InquiryId, CustomerId, NextCallDate, 
				Comment, IsCallFinished, UpdatedOn, UpdatedBy
			) 
			VALUES
			( 
				@InquiryId, @CustomerId, @NextCallDate, 
				@Comment, @IsCallFinished, @UpdatedOn, @UpdatedBy
			)
		END
	
END




