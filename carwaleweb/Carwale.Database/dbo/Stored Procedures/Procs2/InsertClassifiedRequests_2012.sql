IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClassifiedRequests_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClassifiedRequests_2012]
GO
	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE    PROCEDURE [dbo].[InsertClassifiedRequests_2012]
	@SellInquiryId		NUMERIC,	-- Sell Inquiry Id
	@CustomerId		NUMERIC,	-- customer ID
	@Comments		VARCHAR(500),
	@RequestDateTime	DATETIME,	-- Entry Date
	@InquiryId		NUMERIC OUTPUT

 AS
	BEGIN
		-- Check if user is already shown interest
		SET @InquiryId = (SELECT TOP 1 ID FROM ClassifiedRequests WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)
		
		If @InquiryId IS NULL
		BEGIN
			INSERT INTO ClassifiedRequests(SellInquiryId, CustomerId, Comments, RequestDateTime)
			VALUES (@SellInquiryId,@CustomerId, @Comments, @RequestDateTime)
			
			SET @InquiryId = SCOPE_IDENTITY()
		END
	END

	

