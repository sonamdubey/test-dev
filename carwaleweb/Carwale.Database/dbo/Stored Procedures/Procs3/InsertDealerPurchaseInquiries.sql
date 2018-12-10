IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertDealerPurchaseInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertDealerPurchaseInquiries]
GO

	-- =============================================
-- Author:		Ashish Ambokar
-- Create date: 2-4-2012
-- Description:	To insert dealer purchase inquiries
-- =============================================
CREATE    PROCEDURE [dbo].[InsertDealerPurchaseInquiries]  
 @SellInquiryId  NUMERIC, -- Sell Inquiry Id  
 @CustomerId  NUMERIC, -- customer ID   
 @RequestDateTime DATETIME, -- Entry Date  
 @InquiryId  NUMERIC OUTPUT  
  
 AS  
 BEGIN  
  -- Check if user is already shown interest  
  SET @InquiryId = (SELECT TOP 1 ID FROM UsedCarPurchaseInquiries WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)  
    
  If @InquiryId IS NULL  
  BEGIN  
   INSERT INTO UsedCarPurchaseInquiries(SellInquiryId, CustomerId, RequestDateTime)  
   VALUES (@SellInquiryId,@CustomerId, @RequestDateTime)  
     
   SET @InquiryId = SCOPE_IDENTITY()  
  END  
 END  
  