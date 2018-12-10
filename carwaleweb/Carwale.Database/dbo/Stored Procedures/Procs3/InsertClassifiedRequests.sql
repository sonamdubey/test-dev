IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClassifiedRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClassifiedRequests]
GO

	-- Modified date:2-4-2013  by Akansha, Added @SourceId--
-- Modified date:23-11-2012  by Manish(AE1665) for response update on livelistings table
-- Modified By: Avishkar on 10-4-2014  To set lead score for all live listings
-- Modified by Aditi Dhaybar on 24/09/2014 for the Lead Tracking source Id
CREATE PROCEDURE [dbo].[InsertClassifiedRequests]
	@SellInquiryId		NUMERIC,	-- Sell Inquiry Id
	@CustomerId		NUMERIC,	-- customer ID
	@Comments		VARCHAR(500),
	@RequestDateTime	DATETIME,	-- Entry Date
	@InquiryId		NUMERIC OUTPUT,
	@SellerType  varchar(1)=2 , ---individuals only ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table
	@SourceId SMALLINT = 1, --Added by Akansha--
	@IPAddress VARCHAR(20)= NULL,
	@LTsrc VARCHAR(100)= NULL --Added by Aditi Dhaybar on 24/09/2014 for the Lead Tracking source Id
	

 AS
	BEGIN

	    
		-- Check if user is already shown interest
		SET @InquiryId = (SELECT TOP 1 ID FROM ClassifiedRequests WITH (NOLOCK) WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)
		
		
		If @InquiryId IS NULL
		BEGIN
			INSERT INTO ClassifiedRequests(SellInquiryId, CustomerId, Comments,SourceId, RequestDateTime,IPAddress,LTSrc)
			VALUES (@SellInquiryId,@CustomerId, @Comments, @SourceId, @RequestDateTime,@IPAddress,@LTsrc)

			--Modified By: Avishkar on 10-4-2014  To set lead score for all live listings	       
			
           exec UsedCarResponseUpdate  @SellInquiryId,@SellerType    ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table
		
			SET @InquiryId = SCOPE_IDENTITY()
		END
	END


	/****** Object:  StoredProcedure [dbo].[InsertUsedCarPurchaseInquiry]    Script Date: 10/3/2014 11:43:19 AM ******/
SET ANSI_NULLS ON

