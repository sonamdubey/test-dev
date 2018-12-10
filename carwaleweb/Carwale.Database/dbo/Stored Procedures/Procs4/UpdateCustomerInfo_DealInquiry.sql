IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerInfo_DealInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerInfo_DealInquiry]
GO

	-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 1st april, 2016
-- Description:	Update DealInquiries table from booking page when the user edits the personal details
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerInfo_DealInquiry]
	@InquiryId              INT,
	@CustomerName			VarChar(50),
	@CustomerEmail			VarChar(100),
	@CustomerMobile			VarChar(10)

AS
BEGIN

	UPDATE DealInquiries 
	SET
	    CustomerName = @CustomerName,
	    CustomerEmail = @CustomerEmail,
		CustomerMobile = @CustomerMobile
	WHERE ID = @InquiryId	

END
