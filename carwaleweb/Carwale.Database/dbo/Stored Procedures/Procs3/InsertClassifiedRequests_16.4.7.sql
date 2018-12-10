IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertClassifiedRequests_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertClassifiedRequests_16]
GO

	
-- Modified date:2-4-2013  by Akansha, Added @SourceId--
-- Modified date:23-11-2012  by Manish(AE1665) for response update on livelistings table
-- Modified By: Avishkar on 10-4-2014  To set lead score for all live listings
-- Modified by Aditi Dhaybar on 24/09/2014 for the Lead Tracking source Id
--Modified by Aditi Dhaybar on 24/12/2014 for storing the carwale cookie
-- Modified by Navead Kazi on 28/10/2015 for capturing utma and utmz GA cookie
-- Modified by Purohith Guguloth on 06th Nov,2015 for Page wise tracking
-- Modified by Navead Kazi on 05/01/2016 - Added an out parameter for sent sms
-- Modified by Afrose on 14th April 2016, added input parameter IMEI 
 --Modified by Prachi Phalak on 26/04/2016-Added notificationId for app notification.
CREATE PROCEDURE [dbo].[InsertClassifiedRequests_16.4.7]
	@SellInquiryId		INT,	-- Sell Inquiry Id
	@CustomerId		INT,	-- customer ID
	@Comments		VARCHAR(500),
	@RequestDateTime	DATETIME,	-- Entry Date
	@InquiryId		INT OUTPUT,
	@SellerType  varchar(1)=2 , ---individuals only ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table
	@SourceId SMALLINT = 1, --Added by Akansha--
	@IPAddress VARCHAR(20)= NULL,
	@LTsrc VARCHAR(100)= NULL, --Added by Aditi Dhaybar on 24/09/2014 for the Lead Tracking source Id
	@Cwc VARCHAR(100)= NULL, --Added by Aditi Dhaybar on 24/12/2014 for storing the carwale cookie
	@UtmaCookie VARCHAR(500) = NULL,  --Added by Navead Kazi on 28/10/2015 for capturing GA cookie
	@UtmzCookie VARCHAR(500) = NULL,   --Added by Navead Kazi on 28/10/2015 for capturing GA cookie
	@OriginId INT = NULL,      --Added by Purohith Guguloth on 06th Nov,2015 for Page wise tracking
	@SentSMS BIT OUTPUT,-- Modified by Navead Kazi on 05/01/2016 - Added an out parameter for sent sms
	@IMEICode VARCHAR(50)=NULL, --Added by Afrose
	@notificationId TINYINT = 0 --Modified by Prachi Phalak on 26/04/2016-Added notificationId for app notification.
 AS
	BEGIN

	   
		-- Check if user is already shown interest
		SET @InquiryId = (SELECT TOP 1 ID FROM ClassifiedRequests WITH (NOLOCK) WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)

		 SET @SentSMS = 0

		
		
		If @InquiryId IS NULL
		BEGIN
			INSERT INTO ClassifiedRequests(SellInquiryId, CustomerId, Comments,SourceId, RequestDateTime,IPAddress,LTSrc,Cwc,UtmaCookie,UtmzCookie,UsedCarPurchaseOriginId,NotificationId,IMEICode)
			VALUES (@SellInquiryId,@CustomerId, @Comments, @SourceId, @RequestDateTime,@IPAddress,@LTsrc,@Cwc,@UtmaCookie,@UtmzCookie,@OriginId,@notificationId,@IMEICode)

			INSERT INTO ClassifiedRequestsSentSMSDetail(
			CustomerID,SellInquiryId,SMSSentDate
			)
			VALUES(
				@CustomerId,@SellInquiryId,@RequestDateTime
			)

			--Modified By: Avishkar on 10-4-2014  To set lead score for all live listings	       
			
           exec UsedCarResponseUpdate  @SellInquiryId,@SellerType    ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table
		
			SET @InquiryId = SCOPE_IDENTITY()
		END
		ELSE
		  BEGIN
		       DECLARE @LatestSMSSent DATETIME 
			  
			  SELECT TOP(1) @LatestSMSSent = SMSSentDate 
		       FROM ClassifiedRequestsSentSMSDetail WITH (NOLOCK)
		      WHERE  CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId
		      ORDER BY ID DESC
			
			IF (@LatestSMSSent >  @RequestDateTime-1 )
			BEGIN
				SET @SentSMS = 1
			END
			ELSE
			BEGIN
				--UPDATE ClassifiedRequests SET SMSSentDate = GETDATE() WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId

				INSERT INTO ClassifiedRequestsSentSMSDetail(
				CustomerID,SellInquiryId,SMSSentDate
				)
				VALUES(
					@CustomerId,@SellInquiryId,@RequestDateTime
				)
				SET  @SentSMS = 0
			END
		  END
	END

