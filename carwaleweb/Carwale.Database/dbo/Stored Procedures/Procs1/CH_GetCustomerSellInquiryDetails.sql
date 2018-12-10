IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_GetCustomerSellInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_GetCustomerSellInquiryDetails]
GO

	
--Author  : Vinay Kumar Prajapati 12th Aug 2014
--Purpose : To get  customer Details From CustomerSellInquiries table.
--Modifier: 
CREATE PROCEDURE [dbo].[CH_GetCustomerSellInquiryDetails]
(
	@InquiryId	NUMERIC
)		
AS

BEGIN			
	--get the details from the CustomerSellInquiries table, assuming that the customer is verified	
	 SELECT CSI.CustomerName,CSI.CustomerEmail,CSI.CustomerMobile 
	 FROM CustomerSellInquiries AS CSI WITH(NOLOCK) 
	 WHERE CSI.ID = @InquiryId	
END
