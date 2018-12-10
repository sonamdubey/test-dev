IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertHighlightedBid]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertHighlightedBid]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertHighlightedBid]
	@DealerId		NUMERIC,	-- Dealer ID
	@CarId			NUMERIC,	-- Car Version Id
	@BidDateTime 		DATETIME,	-- Entry Date
	@Bid			DECIMAL(9),	-- Car Price
	@clearPrevious		BIT
 AS
	
BEGIN
	--If @ClearPrevious = 1
	--BEGIN
		--DELETE FROM HighlightedCarsBidding WHERE DealerId=@DealerId AND 
		--CONVERT( VARCHAR, BidDateTime, 101)=CONVERT(VARCHAR, @BidDateTime, 101)
	--END	
	INSERT INTO HighlightedCarsBidding( DealerID, CarId, Bid, BidDateTime)
	VALUES(@DealerID, @CarId, @Bid, @BidDateTime)
		
END
