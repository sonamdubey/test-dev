IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarQuoteRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarQuoteRequest]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE PROCEDURE [dbo].[InsertNewCarQuoteRequest]
	@CustomerId		NUMERIC,	-- Dealer ID
	@CarVersionId		NUMERIC,	-- Car Version Id
	@RequestDateTime	DATETIME,	-- Entry Date,
	@IsApproved		BIT

 AS
	BEGIN
		INSERT INTO NewCarPriceQuoteRequests(CustomerId, CarVersionId, RequestDateTime,IsApproved)
		VALUES( @CustomerId, @CarVersionId, @RequestDateTime,@IsApproved)
	END
