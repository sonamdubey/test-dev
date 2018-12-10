IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_UpdateCustomerSellInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_UpdateCustomerSellInquiryDetails]
GO

	

CREATE PROCEDURE [dbo].[CH_UpdateCustomerSellInquiryDetails]
  (
	@CustomerSellInquiryId	 NUMERIC,
	@Name			         VARCHAR(100), 
	@Email			         VARCHAR(100), 
	@Mobile		             VARCHAR(50)

	)	
AS			
BEGIN
		UPDATE CustomerSellInquiries SET
			 CustomerName 			= @Name,
			 CustomerEmail          = @Email,
			 CustomerMobile  		= @Mobile
		WHERE 
			ID = @CustomerSellInquiryId			
		
END
