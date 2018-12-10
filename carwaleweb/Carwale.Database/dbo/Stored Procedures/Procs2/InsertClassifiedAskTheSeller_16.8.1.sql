IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClassifiedAskTheSeller_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClassifiedAskTheSeller_16]
GO
	-- Description - Add Message details sent by buyer      
-- Added by Aditi Dhaybar on 24/09/2014 for new ask the seller functionality
-- Modified by Navead Kazi on 29/10/2015 for capturing GA cookie. Added version 15.10.5
-- Modified by Shubham Agarwal on 12/08/2016 - return @AskTheSellerId
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
    
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE    
    
CREATE  PROCEDURE   [dbo].[InsertClassifiedAskTheSeller_16.8.1]
@Name varchar(100),
@Mobile varchar(20),
@Comments varchar(500),
@InquiryId numeric(18,0),
@SellerType varchar(10),
@UtmaCookie varchar(500) = null,	--Added by navead kazi on 29/10/2015 for capturing GA cookie
@UtmzCookie varchar(500) = null,		--Added by navead kazi on 29/10/2015 for capturing GA cookie
@AskTheSellerId INT OUTPUT
 AS
 BEGIN
    INSERT INTO ClassifiedAskTheSeller(Name, Mobile,Comments,InquiryId,SellerType,UtmaCookie,UtmzCookie)
    VALUES( @Name,@Mobile, @Comments, @InquiryId, @SellerType,@UtmaCookie,@UtmzCookie)
	
	SET @AskTheSellerId = SCOPE_IDENTITY()
END
 


