IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertUsedCarPurchaseInquiry_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertUsedCarPurchaseInquiry_16]
GO

	
-- Modified by - Binumon George    
-- Modified Date -16-02-2012    
-- Description - Add new purchase inquiry to TC from Carwale    
--  Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId    
  
-- Modified By - Ashish Ambokar  
-- Modified Date - 2/7/2012  
--Description - SourceId column value is added in UsedCarPurchaseInquiries table  
-- Modified date:23-11-2012  by Manish(AE1665) for response update on livelistings table  
-- Modified by : Manish Chourasiya on 23-04-2014 added with(nolock) keyword wherever not found.
-- Modified by Aditi Dhaybar on 24/09/2014 for the LeadTracking Source Id 
--Modified by Aditi Dhaybar on 24/12/2014 for storing the carwale cookie
-- Modified by Navead Kazi on 28/10/2015 for capturing utma and utmz GA cookie
-- Modified by Purohith Guguloth in 06th Nov,2015 for Page wise tracking
-- Modified by Navead Kazi on 05/01/2016 - Added an out parameter for sent sms
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
    
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE    
    
CREATE  PROCEDURE   [dbo].[InsertUsedCarPurchaseInquiry_16.1.2]    
 @SellInquiryId  INT, -- Sell Inquiry Id    
 @CarModelIds  VARCHAR(100) = Null, -- Model IDs    
 @CarModelNames VARCHAR(500) = NULL, -- Model Names    
 @CustomerId  INT, -- Customer Id    
 @YearFrom  INT = NULL, -- Car Year From    
 @YearTo  INT = NULL, -- Car Year To    
 @BudgetFrom  NUMERIC = NULL, -- Budget From    
 @BudgetTo  NUMERIC = NULL, -- Budget To    
 @MileageFrom  NUMERIC = NULL, -- Mileage From    
 @MileageTo  NUMERIC = NULL, -- Mileage To    
 @NoOfCars  INT = 1,  -- How Many cars customer intend to buy    
 @BuyTime  VARCHAR(20) = NULL, -- When customer intend to buy? i time-frame    
 @RequestDateTime DATETIME, -- Entry Date    
 @Comments   VARCHAR(2000) = NULL,    
 @IsApproved  BIT = 1,  --- Whether Inquiry is verified or not. If admin submits the query this value should be 1, 0 otherwise.     
 @SourceId SMALLINT = 1,  
 @InquiryId  INT OUTPUT, --id of the inquiry just submitted    
 @SellerType  varchar(1)=1 , ---dealers only ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table 
 @IPAddress VARCHAR(20) = NULL,
 @LTsrc VARCHAR(100) = NULL,  --Added by Aditi Dhaybar on 24/09/2014 for the LeadTracking Source Id 
 @Cwc VARCHAR(100)= NULL, --Added by Aditi Dhaybar on 24/12/2014 for storing the carwale cookie
 @UtmaCookie VARCHAR(500) = NULL,  --Added by Navead Kazi on 28/10/2015 for capturing GA cookie
 @UtmzCookie VARCHAR(500) = NULL,   --Added by Navead Kazi on 28/10/2015 for capturing GA cookie
 @OriginId INT = NULL,  --Added by Purohith Guguloth on 06th Nov,2015 for Page wise tracking
 @SentSMS BIT OUTPUT -- Modified by Navead Kazi on 05/01/2016 - Added an out parameter for sent sms
 AS    
 BEGIN 
  
	 
 SET @SentSMS = 0

 
            
  -- Check if user is already shown interest    
  If NOT EXISTS(SELECT ID FROM UsedCarPurchaseInquiries WITH (NOLOCK) WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId)    
  BEGIN      
      
      BEGIN TRY    
          
    INSERT INTO UsedCarPurchaseInquiries(CustomerId, SellInquiryId,     
     YearFrom, YearTo, KmFrom, KmTo, PriceFrom, PriceTo,    
     NoOfCars, BuyTime, CarModelIds, CarModelNames, Comments, RequestDateTime, IsApproved, SourceId,IPAddress,LTSrc,Cwc,UtmaCookie,UtmzCookie,UsedCarPurchaseOriginId)    
    VALUES( @CustomerId, @SellInquiryId,     
     @YearFrom, @YearTo, @MileageFrom, @MileageTo, @BudgetFrom, @BudgetTo,    
     @NoOfCars, @BuyTime, @CarModelIds, @CarModelNames,  @Comments,  @RequestDateTime,@IsApproved, @SourceId,@IPAddress,@LTsrc,@Cwc,
	 @UtmaCookie,@UtmzCookie,@OriginId)    
    
	
	INSERT INTO UsedCarPurchaseInquiriesSentSMSDetail(
	CustomerID,SellInquiryId,SMSSentDate
	)
	VALUES(
		@CustomerId,@SellInquiryId,@RequestDateTime
	)

     
   exec UsedCarResponseUpdate  @SellInquiryId,@SellerType    ---Line add by manish(AE1665)on 23-11-2012 for response update on livelistings table  
     
    SET @InquiryId =  SCOPE_IDENTITY()    
        
    ---- Check if this car uploaded through Trading Cars application    
    --declare @StockId numeric, @BranchId int, @dealerid numeric    
        
    --select @StockId = si.TC_StockId, @BranchId = st.BranchId, @dealerid = si.DealerId     
    --from SellInquiries si, TC_Stock st where si.TC_StockId = st.Id and si.ID = @SellInquiryId    
        
    --if @StockId is not null    
    --BEGIN    
    -- declare @customer varchar(50), @email varchar(100), @mobile varchar(12), @current_time datetime = getdate(), @id numeric    
         
    -- -- If yes, send this inquiry back to thet dealer    
    -- select @customer=cs.Name, @mobile=cs.Mobile, @email=cs.email from Customers cs where Id = @CustomerId    
         
    -- -- Push this inquiry to trading car software by calling common procedure TC_AddBuyerInquiries          
         
           
    -- EXECUTE  TC_AddBuyerInquiries @BranchId =@dealerid,@StockId=@StockId,@CustomerName=@customer,@Email=@email,@Mobile =@mobile,    
    --        @Location=NULL,@Buytime=NULL,@CustomerComments=NULL,@Comments =NULL,    
    --        @InquiryStatus =NUll,@NextFollowup =NULL,@AssignedTo =NULL,@InquirySource =1,@UserId =NULL,@UsedCarPurchaseInquiryId=@InquiryId    
    --        ----  Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId    
                
           
             
    --END    
  END TRY      
       
   BEGIN CATCH       
     INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)      
     VALUES('InsertUsedCarPurchaseInquiry',ERROR_MESSAGE(),GETDATE())      
    --SELECT ERROR_NUMBER() AS ErrorNumber;      
   END CATCH;      
      
  END 
  -- Modified by Navead Kazi on 05/01/2016 - Added an out parameter for sent sms
  ELSE
  BEGIN
	DECLARE @LatestSMSSent DATETIME
		SELECT TOP(1) @LatestSMSSent = SMSSentDate 
		 FROM UsedCarPurchaseInquiriesSentSMSDetail WITH (NOLOCK)
		 WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId
		 ORDER BY ID DESC
	IF (@LatestSMSSent >  @RequestDateTime-1)
	BEGIN
		SET @SentSMS = 1
	END
	ELSE
	BEGIN
		--UPDATE UsedCarPurchaseInquiries SET SMSSentDate = GETDATE() WHERE CustomerID = @CustomerId AND SellInquiryId = @SellInquiryId

		INSERT INTO UsedCarPurchaseInquiriesSentSMSDetail(
		CustomerID,SellInquiryId,SMSSentDate
		)
		VALUES(
			@CustomerId,@SellInquiryId,@RequestDateTime
		)
		
		SET  @SentSMS = 0
	END
  END    
 END

