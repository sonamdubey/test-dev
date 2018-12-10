IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PlaceDealerBid]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PlaceDealerBid]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [PlaceDealerBid]
	@DealerId		NUMERIC,	-- Dealer ID
	@SellInquiryId		NUMERIC,	-- Customer Sell InquiryId
	@BidAmount		NUMERIC,	-- Bid Amount
	@LastUpdated 		DATETIME	-- Entry Date

 AS

BEGIN

	-- Delete Current Bid, if is there!
	DELETE FROM DealerBidsForCustomerSellInquiries WHERE DealerId=@DealerId AND SellInquiryId=@SellInquiryId

	If @BidAmount <> 0 
	BEGIN
		INSERT INTO DealerBidsForCustomerSellInquiries(DealerId,SellInquiryId,BidAmount,LastUpdated)
		VALUES(@DealerId,@SellInquiryId,@BidAmount,@LastUpdated)
	END
	
END
