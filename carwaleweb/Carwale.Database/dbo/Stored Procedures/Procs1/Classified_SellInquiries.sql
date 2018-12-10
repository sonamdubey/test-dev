IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_SellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_SellInquiries]
GO

	
--Modify by umesh on 13 dec 12
--Added new parameters for new design
-- Modified By : Akansha on 14.02.2014
-- Description : Changed the data type for @Mileage from int to varchar(50)
--Modified By: Vinay Kumar 22nd May 2014 for Adding WITH(NOLOCK).
--           : Vinay Kumar 17th july 2014 To Resolve problem of  30 days added in  ClassifiedExpiryDate  
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE        
        
CREATE  PROCEDURE [dbo].[Classified_SellInquiries]        

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
	 @PinCode Int =NULL,
	 @ReasonForSelling VARCHAR(500),
	--New parameter added as per new design
	 @IntColor VARCHAR(30) = NULL ,
	 @Mileage  VARCHAR(50) = NULL ,
	 
	 @ID   NUMERIC OUTPUT        
        
 AS        
	DECLARE           
		@LastBidDate  DATETIME,        
		@ClassifiedExpiry DATETIME,        
		@FreeInquiryLeft INT,        
		@PaidInquiryLeft INT, 
		@RegState	CHAR(4),
		@CustomerName VARCHAR(100), -- Deepak Added 2013-11-30 to get CustomerEmail, CustomerName, CustomerMobile 
		@CustomerEmail VARCHAR(100),
		@CustomerMobile VARCHAR(20) 
        
	BEGIN  
 
		SET @RegState=substring(Upper(dbo.RemoveSpecialChars(@RegNo)),1,4)
		if LEN(@RegState)<4 
		set @RegState=NULL    
 
		-- Deepak Added 2013-11-30 to get CustomerEmail, CustomerName, CustomerMobile 
		SELECT @CustomerName = Name, @CustomerEmail = email,    @CustomerMobile = Mobile    
		FROM Customers WITH(NOLOCK) 
		WHERE Id = @CustomerId  
   
		IF @SellInquiryId = -1      
			BEGIN            
				--fetch the max paid inquiry and the max free inquiry from the AllowedInquiriesMaster table for the  ConsumerType = 2 for the customers        
				SELECT @FreeInquiryLeft = MaxFreeInquiry, @PaidInquiryLeft = MaxPaidInquiry       
				FROM AllowedInquiriesMaster WITH(NOLOCK) WHERE ConsumerType = 2      
      
				--  Classified Expiry date will be 30 days from date of listing      
				SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)           
      
				INSERT INTO CustomerSellInquiries( CustomerId, CityId, CarVersionId, CarRegNo, EntryDate, Price,        
						MakeYear, Kilometers,Color, ColorCode, Comments, ForwardDealers, ListInClassifieds,IsApproved,        
						LastBidDate, ClassifiedExpiryDate, PaidInqLeft, FreeInqLeft, PackageType, SourceId,CarRegState, PinCode, ReasonForSelling,
						CustomerName,CustomerEmail,CustomerMobile)         
				VALUES(@CustomerId, @CityId, @CarVersionId, @RegNo, @RequestDateTime, @Price,        
						@MakeYear, @Kms, @Color, @ColorCode, @Comments, @SendDealers, @ListInClassifieds,        
						@IsApproved, @LastBidDate, @ClassifiedExpiry, @PaidInquiryLeft, @FreeInquiryLeft, 1, @SourceId,@RegState, @PinCode, @ReasonForSelling,
						@CustomerName,@CustomerEmail,@CustomerMobile)      
        
				SET @ID = SCOPE_IDENTITY()      
      
				INSERT INTO CustomerSellInquiryDetails(InquiryId, RegistrationPlace, Insurance, InsuranceExpiry, Owners, Tax)        
				VALUES (@ID, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, @Tax)
			END        
		ELSE        
			BEGIN 
			    -- Vinay Kumar Prajapati 17th july 2014 to resolving  problem of expiry date(added 30 days in ClassifiedExpiryDate) on updation 
				       
				--SELECT @ClassifiedExpiry = ClassifiedExpiryDate FROM CustomerSellInquiries WITH(NOLOCK) WHERE Id=@SellInquiryId          
      
				---- Check if the inquiry is in bidding ?        
				--If DATEDIFF(d, @RequestDateTime, @ClassifiedExpiry) < 30 OR @ClassifiedExpiry IS NULL        
				--	BEGIN        
				--		SET @ClassifiedExpiry  = DATEADD(DAY,30,@RequestDateTime)
				--	END         
      
			UPDATE CustomerSellInquiries SET 
					CityId = ISNULL(@CityId,CityId), 
					CarVersionId = ISNULL(@CarVersionId,CarVersionId),         
					CarRegNo = ISNULL(@RegNo,CarRegNo),
					Price = ISNULL(@Price,Price),
					MakeYear = ISNULL(@MakeYear,MakeYear),
					Kilometers = ISNULL(@Kms,Kilometers),	        
					Color = ISNULL(@Color,Color),
					ColorCode = ISNULL(@ColorCode,ColorCode),
					Comments = ISNULL(@Comments,Comments),
					--Vinay kumar 17th july 2014 
					--ClassifiedExpiryDate = ISNULL(@ClassifiedExpiry,ClassifiedExpiryDate),
					CarRegState = ISNULL(@RegState,CarRegState),
					PinCode = ISNULL(@PinCode,PinCode),
					ReasonForSelling = ISNULL(@ReasonForSelling,ReasonForSelling),
					CustomerId=@CustomerId,
					CustomerName=@CustomerName,
					CustomerEmail=@CustomerEmail,
					CustomerMobile=@CustomerMobile
				WHERE ID = @SellInquiryId   
				  
             SELECT CSI.InquiryId FROM CustomerSellInquiryDetails AS CSI WITH(NOLOCK)   WHERE InquiryId = @SellInquiryId
			  
			 IF @@ROWCOUNT <> 0
				 BEGIN 
					  UPDATE CustomerSellInquiryDetails SET 
							RegistrationPlace = ISNULL(@RegPlace,RegistrationPlace),
							Insurance = ISNULL(@Insurance,Insurance),       
							InsuranceExpiry = ISNULL(@InsuranceExpiry,InsuranceExpiry), 
							Owners = ISNULL(@Owners,Owners), 
							Tax = ISNULL(@Tax,Tax),
							CityMileage = ISNULL(@Mileage,CityMileage),
							InteriorColor = ISNULL(@IntColor,InteriorColor)
					  WHERE InquiryId = @SellInquiryId   
				  END 
			  ELSE
				  BEGIN
					  INSERT INTO CustomerSellInquiryDetails(InquiryId,RegistrationPlace,Insurance,InsuranceExpiry,Owners,Tax,CityMileage,InteriorColor) 
					  VALUES(@SellInquiryId,@RegPlace,@Insurance,@InsuranceExpiry,@Owners,@Tax,@Mileage,@IntColor)
				  END   
    
			SET @ID = @SellInquiryId      
	END         
 END        
     