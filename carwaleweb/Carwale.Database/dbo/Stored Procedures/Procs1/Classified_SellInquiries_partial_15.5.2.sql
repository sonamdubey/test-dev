IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiries_partial_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiries_partial_15]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE  
-- Avishkar Added 16-5-2013 @PackageId to show customer's package      
-- Amit Verma 22-5-2013 @Referrer to show referrer  
-- Avishkar Added 2013-11-11 to get CustomerEmail, CustomerName, CustomerMobile 
-- Aditi Added AreaId on 3/2/2015
CREATE PROCEDURE [dbo].[Classified_SellInquiries_partial_15.5.2]        

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
--Added by Aditi Dhaybar on 3/2/2015 to add AreaId 
 ,@AreaName varchar(50)
 ,@ShowContactDetails  BIT
 AS        
 DECLARE           
  @LastBidDate  DATETIME,        
  @ClassifiedExpiry DATETIME,        
  @FreeInquiryLeft INT,        
  @PaidInquiryLeft INT,
  @CustomerName VARCHAR(100), -- Avishkar Added 2013-11-11 to get CustomerEmail, CustomerName, CustomerMobile 
  @CustomerEmail VARCHAR(100),
  @CustomerMobile VARCHAR(20),
  @AreaId  NUMERIC(18,0)
  
        
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
  
  --Added by Aditi Dhaybar on 3/2/2015 to add AreaId 
  SELECT @AreaId = ID 
  FROM Areas WITH(NOLOCK) 
  WHERE Name = @AreaName AND PinCode = @PinCode;
         
 -- Avishkar Added 16-5-2013 @PackageId to show customer's package      
 -- Avishkar Added 2013-11-11 to set CustomerEmail, CustomerName, CustomerMobile
 --Aditi Dhaybar Added AreaId on 3/2/2015    
  INSERT INTO CustomerSellInquiries( CustomerId, CityId, CarVersionId, EntryDate, Price, MakeYear, Kilometers, ForwardDealers, 
  ListInClassifieds,IsApproved,LastBidDate,  PaidInqLeft, FreeInqLeft, PackageType, SourceId,PinCode,PackageId,Referrer,IPAddress,
  CustomerName,CustomerEmail,CustomerMobile,AreaId,IsShowContact)            
  VALUES(@CustomerId, @CityId, @CarVersionId, @RequestDateTime, @Price,@MakeYear, @Kms, @SendDealers, @ListInClassifieds,        
  @IsApproved, @LastBidDate, @PaidInquiryLeft, @FreeInquiryLeft, 1, @SourceId,@PinCode,@PackageId,@Referrer,@IPAddress,
  @CustomerName,@CustomerEmail,@CustomerMobile,@AreaId,@ShowContactDetails)  
         
        
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
  --Aditi Dhaybar Added AreaId o show AreaName on 3/2/2015   
      
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
	  CustomerMobile=@CustomerMobile,
	  IsShowContact = @ShowContactDetails
  WHERE ID=@SellInquiryId 
          
    
  SET @ID = @SellInquiryId      
  END          
 END        
     




