IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiries_Bak]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiries_Bak]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE        
        
CREATE    PROCEDURE [dbo].[Classified_SellInquiries_Bak]        
 @SellInquiryId  NUMERIC, -- Sell Inquiry Id        
 @CustomerId  NUMERIC, -- customer ID        
 @CityId  INT,      
 @CarVersionId  NUMERIC, -- Car Version Id        
 @RequestDateTime DATETIME, -- Entry Date        
 @MakeYear  DATETIME,        
 @RegNo  VARCHAR(50),        
 @Kms   NUMERIC,        
 @Price   NUMERIC,        
 @Color   VARCHAR(50),        
 @ColorCode VARCHAR(6),     
 @Comments  VARCHAR(500),        
 @SendDealers  BIT,        
 @ListInClassifieds BIT,        
 @IsApproved  BIT,      
       
 -- additional details      
 @RegPlace  VARCHAR(50),        
 @Owners  NUMERIC,        
 @Tax   VARCHAR(50),        
 @Insurance  VARCHAR(50),        
 @InsuranceExpiry DATETIME,
 @SourceId SMALLINT,          
 @ID   NUMERIC OUTPUT        
        
 AS        
 DECLARE           
  @LastBidDate  DATETIME,        
  @ClassifiedExpiry DATETIME,        
  @FreeInquiryLeft INT,        
  @PaidInquiryLeft INT      
        
 BEGIN      
        
 IF @SellInquiryId = -1      
  BEGIN            
  --fetch the max paid inquiry and the max free inquiry from the AllowedInquiriesMaster table for the  ConsumerType = 2 for the customers        
  SELECT @FreeInquiryLeft = MaxFreeInquiry, @PaidInquiryLeft = MaxPaidInquiry       
  FROM AllowedInquiriesMaster WHERE ConsumerType = 2      
      
  --  Classified Expiry date will be 30 days from date of listing      
  SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)           
      
  INSERT INTO CustomerSellInquiries( CustomerId, CityId, CarVersionId, CarRegNo, EntryDate, Price,        
  MakeYear, Kilometers,Color, ColorCode, Comments, ForwardDealers, ListInClassifieds,IsApproved,        
  LastBidDate, ClassifiedExpiryDate, PaidInqLeft, FreeInqLeft, PackageType, SourceId)         
  VALUES(@CustomerId, @CityId, @CarVersionId, @RegNo, @RequestDateTime, @Price,        
  @MakeYear, @Kms, @Color, @ColorCode, @Comments, @SendDealers, @ListInClassifieds,        
  @IsApproved, @LastBidDate, @ClassifiedExpiry, @PaidInquiryLeft, @FreeInquiryLeft, 1, @SourceId)      
        
  SET @ID = SCOPE_IDENTITY()      
      
  INSERT INTO CustomerSellInquiryDetails(InquiryId, RegistrationPlace, Insurance, InsuranceExpiry, Owners, Tax)        
  VALUES (@ID, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, @Tax)            
  END        
 ELSE        
  BEGIN        
  SELECT @ClassifiedExpiry = ClassifiedExpiryDate FROM CustomerSellInquiries WHERE Id=@SellInquiryId          
      
  -- Check if the inquiry is in bidding ?        
  If DATEDIFF(d, @RequestDateTime, @ClassifiedExpiry) < 30 OR @ClassifiedExpiry IS NULL        
   BEGIN        
    SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)          
   END         
      
  UPDATE CustomerSellInquiries SET CityId = @CityId, CarVersionId=@CarVersionId,         
  CarRegNo=@RegNo, Price=@Price, MakeYear=@MakeYear, Kilometers=@Kms,        
  Color=@Color, ColorCode=@ColorCode, Comments=@Comments, ClassifiedExpiryDate=@ClassifiedExpiry
  WHERE ID=@SellInquiryId      
          
  UPDATE CustomerSellInquiryDetails SET RegistrationPlace = @RegPlace, Insurance = @Insurance,       
  InsuranceExpiry = @InsuranceExpiry, Owners = @Owners, Tax = @Tax         
  WHERE InquiryId = @SellInquiryId      
    
  SET @ID = @SellInquiryId      
  END          
 END        
     
     
 --drop proc Classified_SellInquiries
