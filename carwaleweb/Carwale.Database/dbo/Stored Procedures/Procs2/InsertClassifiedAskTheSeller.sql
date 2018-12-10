IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClassifiedAskTheSeller]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClassifiedAskTheSeller]
GO

	-- Description - Add Message details sent by buyer      
-- Added by Aditi Dhaybar on 24/09/2014 for new ask the seller functionality
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
    
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE    
    
CREATE  PROCEDURE   [dbo].[InsertClassifiedAskTheSeller]
@Name varchar(100),
@Mobile varchar(20),
@Comments varchar(500),
@InquiryId numeric(18,0),
@SellerType varchar(10)
 AS    
 BEGIN         
    INSERT INTO ClassifiedAskTheSeller(Name, Mobile,Comments,InquiryId,SellerType)    
    VALUES( @Name,@Mobile, @Comments, @InquiryId, @SellerType)    
	  
  END     
 












