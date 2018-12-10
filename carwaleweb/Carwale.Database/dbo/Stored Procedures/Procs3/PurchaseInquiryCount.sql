IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PurchaseInquiryCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PurchaseInquiryCount]
GO

	
--THIS PROCEDURE RETURNS THE COUNT FOR THE ID PASSED FROM THE Purchase Inquiries


CREATE PROCEDURE [dbo].[PurchaseInquiryCount]
	@DealerID		NUMERIC,	--ID.		        	
	@COUNT	INTEGER OUTPUT	--return value, COUNT
 AS
	DECLARE @TEMPCOUNT AS INTEGER
BEGIN
	
	SELECT @TEMPCOUNT = COUNT(ID) FROM PurchaseInquiries  WHERE DealerID = @DealerID AND IsArchived = 0
	
	SET @COUNT = @TEMPCOUNT	
	
END
