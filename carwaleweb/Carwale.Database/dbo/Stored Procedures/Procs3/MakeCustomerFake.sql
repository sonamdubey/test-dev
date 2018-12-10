IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MakeCustomerFake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MakeCustomerFake]
GO

	--THIS PROCEDURE IS FOR MAKING THE CUSTOMER AND ALL ITS INQUIRIES AS FAKE.
CREATE PROCEDURE [dbo].[MakeCustomerFake]
	@CustomerId		NUMERIC
AS
	DECLARE 	@TempId 		AS NUMERIC
			
BEGIN
	
	BEGIN TRANSACTION TRANSCUSTOMERFAKE
	
		--fetch the TEMPID
		SELECT @TempId = IsNull(TempId, -1)  FROM Customers  with (NOLOCK)WHERE ID = @CustomerId
		
		PRINT @TempId
			
		--NOW MAKE THE ISVERIFIED FLAG ANAD THE 
		--FAKE FLAG TO TRUE AND DELETE THENENTRY FROM THE TEMPCUSTOMERS TABLE AND ALSO MAKE ALL 
		--THE INQUIRIES TO BE FAKE 
		
		UPDATE Customers SET IsFake = 1, IsVerified = 1 WHERE Id = @CustomerId			
		DELETE FROM TempCustomers WHERE Id = @TempId
		--also update all the inquiries
		-- changed by Navead for mysql migration
		-- UPDATE CustomerSellInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
		--- UPDATE UsedCarPurchaseInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
		UPDATE CustomerTradeinInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
		UPDATE NewCarPurchaseInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
		UPDATE NewCarPriceQuoteRequests SET IsFake = 1 WHERE CustomerId = @CustomerId
		UPDATE FinanceInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
		UPDATE InsuranceInquiries SET IsFake = 1 WHERE CustomerId = @CustomerId
	COMMIT TRANSACTION TRANSCUSTOMERFAKE
END
select * from customers

