IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Dealer_SaveListingFollowup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Dealer_SaveListingFollowup]
GO

	

CREATE PROCEDURE [dbo].[Dealer_SaveListingFollowup]
	@LogId          NUMERIC,
	@CustomerID     NUMERIC,
	@NextCallDate   DATETIME,
	@IsConverted	BIT,
	@Comments		VARCHAR(150),
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC 
AS

BEGIN

	UPDATE Dealer_ListingFollowUp 
	SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, 
		NextCallDate = @NextCallDate, Comments = Comments + '<BR>' + @Comments,
		IsConverted = @IsConverted
	WHERE LogId  = @LogId 
	
	IF @@ROWCOUNT = 0
		BEGIN
			
			INSERT INTO Dealer_ListingFollowUp
			(
				LogId, CustomerId, NextCallDate, 
				Comments, IsConverted, UpdatedOn, UpdatedBy
			) 
			VALUES
			( 
				@LogId, @CustomerId, @NextCallDate, 
				@Comments, @IsConverted, @UpdatedOn, @UpdatedBy
			)
		END
	
END

