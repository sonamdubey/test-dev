IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiries_partial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiries_partial]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE  
-- Avishkar Added 16-5-2013 @PackageId to show customer's package      
-- Amit Verma 22-5-2013 @Referrer to show referrer  
-- Avishkar Added 2013-11-11 to get CustomerEmail, CustomerName, CustomerMobile 
CREATE PROCEDURE [dbo].[Classified_SellInquiries_partial]        

 @SellInquiryId  NUMERIC, -- Sell Inquiry Id        
 @CustomerId  NUMERIC, -- customer ID        
 @CityId  INT,      
 @CarVersionId  NUMERIC, -- Car Version Id        
 @RequestDateTime DATETIME, -- Entry Date        
 @MakeYear  DATETIME, 
 @Kms   NUMERIC,
 @Price   NUMERIC,                
 @ListInClassifieds BIT,        
 @IsApproved  BIT,      
 @SourceId SMALLINT, 
 @SendDealers  BIT,  
 @PinCode Int =NULL,  
 @PackageId INT, -- Avishkar Added 16-5-2013 to show customer's package      
 @ID   NUMERIC OUTPUT,
 @Referrer nvarchar(max) = null,
 @IPAddress varchar(150) = null
 AS        
 DECLARE           
  @LastBidDate  DATETIME,        
  @ClassifiedExpiry DATETIME,        
  @FreeInquiryLeft INT,        
  @PaidInquiryLeft INT,
  @CustomerName VARCHAR(100), -- Avishkar Added 2013-11-11 to get CustomerEmail, CustomerName, CustomerMobile 
  @CustomerEmail VARCHAR(100),
  @CustomerMobile VARCHAR(20)
  
        
 BEGIN 

 -- Avishkar Added 2013-11-11 to get CustomerEmail, CustomerName, CustomerMobile 
  SELECT @CustomerName = Name, @CustomerEmail = email,    @CustomerMobile = Mobile    
  FROM Customers WITH(NOLOCK) 
  WHERE Id = @CustomerId  
        
 IF @SellInquiryId = -1      
  BEGIN            
  --fetch the max paid inquiry and the max free inquiry from the AllowedInquiriesMaster table for the  ConsumerType = 2 for the customers        
  SELECT @FreeInquiryLeft = MaxFreeInquiry, @PaidInquiryLeft = MaxPaidInquiry       
  FROM AllowedInquiriesMaster WITH(NOLOCK) WHERE ConsumerType = 2      
      
  --  Classified Expiry date will be 30 days from date of listing      
  --SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)
  
  
         
 -- Avishkar Added 16-5-2013 @PackageId to show customer's package      
 -- Avishkar Added 2013-11-11 to set CustomerEmail, CustomerName, CustomerMobile  
  INSERT INTO CustomerSellInquiries( CustomerId, CityId, CarVersionId, EntryDate, Price, MakeYear, Kilometers, ForwardDealers, 
  ListInClassifieds,IsApproved,LastBidDate,  PaidInqLeft, FreeInqLeft, PackageType, SourceId,PinCode,PackageId,Referrer,IPAddress,
  CustomerName,CustomerEmail,CustomerMobile)            
  VALUES(@CustomerId, @CityId, @CarVersionId, @RequestDateTime, @Price,@MakeYear, @Kms, @SendDealers, @ListInClassifieds,        
  @IsApproved, @LastBidDate, @PaidInquiryLeft, @FreeInquiryLeft, 1, @SourceId,@PinCode,@PackageId,@Referrer,@IPAddress,
  @CustomerName,@CustomerEmail,@CustomerMobile)  
         
        
  SET @ID = SCOPE_IDENTITY()      
              
  END        
 ELSE        
  BEGIN   
  
  -- Avishkar commented as ClassifiedExpiryDate is again set in SP UpgradePackageTypeToListingType	      
  
  --SELECT @ClassifiedExpiry = ClassifiedExpiryDate FROM CustomerSellInquiries WITH(NOLOCK) WHERE Id=@SellInquiryId          
       
  ---- Check if the inquiry is in bidding ?        
  --If DATEDIFF(d, @RequestDateTime, @ClassifiedExpiry) < 30 OR @ClassifiedExpiry IS NULL        
  -- BEGIN        
  --  SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)          
  -- END         
   
  -- Avishkar Added 2013-11-11 to set CustomerEmail, CustomerName, CustomerMobile 
  -- Avishkar Added 16-5-2013 @PackageId to show customer's package  
      
  UPDATE CustomerSellInquiries 
  SET CityId = @CityId, 
	  CarVersionId=@CarVersionId,         
	  MakeYear=@MakeYear,
	  Kilometers=@Kms,
	  --ClassifiedExpiryDate=@ClassifiedExpiry,  Avishkar commented as ClassifiedExpiryDate is again set in SP UpgradePackageTypeToListingType	  
	  PinCode = @PinCode,
	  PackageId=@PackageId,
	  CustomerId=@CustomerId,
	  CustomerName=@CustomerName,
	  CustomerEmail=@CustomerEmail,
	  CustomerMobile=@CustomerMobile	  
  WHERE ID=@SellInquiryId 
          
    
  SET @ID = @SellInquiryId      
  END          
 END        
     

