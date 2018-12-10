IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSellInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSellInquiry]
GO

	-- =============================================
-- ModifiedBy:	Surendra
-- Create date: 15 may 2012
-- Description:	last updated date will changed only in case where ip price is greater then exisiting price
-- ModifiedBy:	Tejashree Patil on 8 Nov 2012 at 4pm Description: @Color VARCHAR(50) from VARCHAR(30)
-- ModifiedBy:	Surendra on 23 Nov 2012 at 3pm Description: Package expiry date update if stock is already there in Sellinquiries
-- ModifiedBy:	Manish Chourasiya(AE1665) on 26 Nov 2012 at 3pm Description: last updated date update if stock is already present in Sellinquiries
-- ModifiedBy:	Satish Sharma  Description: optimized query
-- Modified By : Surendra on 15th Apr  2013, added certification path
-- Modified By:  Manish on 26-07-2013 for maintaining log of the uploaded car
-- Modified By:  Tejashree Patil on 7 Aug 2013, Updated PackageType in SellInquiries table.
-- Modified By  Vivek Gupta on 25/11/13, Added Paramete @Ispremimum to insert/update premium details of dealers
-- Modified By Vivek Gupta on 6th jan 2014, Added parameter @VideoUrl to save youtube link in CarVideos and sellinquiries details
-- Modified By: Manish on 22-04-2014 added WITH (NOLOCK) keyword wherever not found.
-- Modified By Vivek Gupta on 10-11-2014 added @EMI parameter
-- =============================================    
CREATE  PROCEDURE  [dbo].[InsertSellInquiry]

	 @Id   NUMERIC, -- SellInquiry Id. Will be -1 if Its Insertion    
	 @DealerId  NUMERIC, -- Dealer ID    
	 @CarVersionId  NUMERIC, -- Car Version Id    
	 @CarRegNo  VARCHAR(15), -- Car RegistrationNo     
	 @StatusId  NUMERIC, -- Car Status Id    
	 @EntryDate   DATETIME, -- Entry Date    
	 @Price   DECIMAL(9), -- Car Price    
	 @MakeYear  DATETIME, -- Car Make-Year    
	 @Kilometers  NUMERIC, -- Mileage, Kilometers Done    
	 @Color   VARCHAR(50), -- Car Color      -- ModifiedBy:	Tejashree Patil on 8 Nov 2012 at 4pm
	 @ColorCode VARCHAR(6), -- Car Color Code   
	 @Comments  VARCHAR(500), -- Dealer Comments    
	 @ImportChecksum NUMERIC, -- THE CHECKSUM, DEFAULT IS SET TO -1. TO BE USED WHILE IMPORTING DATA    
	 @RecordId  NUMERIC OUTPUT, -- In case of Insertion, It will hold current Record Id.    
	 @Accessories  VARCHAR(2000),    
	 @StockCar  TINYINT, -- Weather accessories are of Stocked Car or customer Sell Inquiry? 0 no 1 yes.    
	 @PackageType  INT,    
	 @PackageExpiryDate DATETIME,	
	    
	 -- sell inquiry details    
	 @Owners  VARCHAR(50), -- No Of Owners    
	 @RegPlace  VARCHAR(50),  -- Registration Place Id    
	 @OneTimeTax    VARCHAR(50),  -- OneTimeTax    
	 @Insurance  VARCHAR(50), -- Insurance    
	 @InsExpiry  DateTime, -- Insurance Expiry    
	 @UpdateTimeStamp DATETIME,      
	 @InteriorColor  VARCHAR(50),   
	 @InteriorColorCode VARCHAR(6),    
	 @CityMileage  VARCHAR(50),     
	 @AdditionalFuel  VARCHAR(50),     
	 @CarDriven  VARCHAR(50),     
	 @Accidental  BIT,     
	 @FloodAffected  BIT,     
	 @Warranties  VARCHAR(500),     
	 @Modifications  VARCHAR(500), 
	 @AirConditioning VARCHAR(50),     
	 @BatteryCondition VARCHAR(50),     
	 @BrakesCondition VARCHAR(50),     
	 @ElectricalsCondition VARCHAR(50),     
	 @EngineCondition VARCHAR(50),     
	 @ExteriorCondition VARCHAR(50),     
	 @SeatsCondition VARCHAR(50),     
	 @SuspensionsCondition VARCHAR(50),     
	 @TyresCondition VARCHAR(50),     
	 @OverallCondition VARCHAR(50),
	 @SafetyFeatures VARCHAR(500) = null,      -- Safety Features
	 @ComfortFeatures VARCHAR(500) = null,     -- Comfort Features
	 @OtherFeatures VARCHAR(500) = null,		-- Other Features
	 @CertificationId	SMALLINT = NULL,
	 @InteriorCondition  VARCHAR(50)=NULL,
	 @IsPremium BIT = 0,-- Modified By  Vivek Gupta on 25/11/13
	 @VideoUrl VARCHAR(20) = NULL ,-- Modified By Vivek Gupta on 6th jan 2014
	 @EMI NUMERIC = NULL  -- Modified By Vivek Gupta on 10-11-2014
 AS    
	 DECLARE     
	  @FreeInquiryLeft INT,    
	  @PaidInquiryLeft INT,
	  @OldPrice as  NUMERIC, --Line add by Manish(AE1665) for implementation of logic of lastupdated date for used car.
	  @OldCommentLength as smallint --Line add by Manish(AE1665) for implementation of logic of lastupdated date for used car.   
BEGIN      
    
	-- Modified By Vivek Gupta on 6th jan 2014
    DECLARE @IsYouTubeVideoApproved BIT = 0	
	IF(@VideoUrl IS NOT NULL)
		SET @IsYouTubeVideoApproved = 1

	IF @Id = -1 AND @ImportChecksum <> -1 -- Maybe Update or Insert. Will have to check    
		BEGIN    
			SELECT @Id=ID FROM SellInquiries WITH (NOLOCK) WHERE DealerId=@DealerId AND ImportChecksum=@ImportChecksum      
			IF @Id IS NULL    
				BEGIN    
					SET @Id = -1    
				END    
		END    
		print  @Id
		If @Id = -1     
			BEGIN    
				--fetch the max paid inquiry and the max free inquiry from the AllowedInquiriesMaster table for the  ConsumerType = 1 for the dealers    
				SELECT @FreeInquiryLeft = MaxFreeInquiry, @PaidInquiryLeft = MaxPaidInquiry     
				FROM AllowedInquiriesMaster WITH (NOLOCK)
				WHERE ConsumerType = 1    
			    
			    DECLARE @CertifiedLogoUrl varchar(200)
				IF(@CertificationId IS NOT NULL)
				BEGIN		
					SELECT @CertifiedLogoUrl=HostURL + DirectoryPath + LogoURL FROM Classified_CertifiedOrg WITH (NOLOCK) WHERE Id=@CertificationId
				END
			
				INSERT INTO SellInquiries( DealerId, CarVersionId, CarRegNo, EntryDate, StatusId, Price,    
				MakeYear, Kilometers,Color,ColorCode, Comments, ImportChecksum, LastUpdated, FreeInqLeft, PaidInqLeft,
				PackageType, PackageExpiryDate, CertificationId,CertifiedLogoUrl,IsPremium, EMI) -- Modified By  Vivek Gupta on 25/11/13    
				VALUES(@DealerId, @CarVersionId, @CarRegNo, @EntryDate, @StatusId, @Price,    
				@MakeYear, @Kilometers, @Color, @ColorCode, @Comments, @ImportChecksum, @EntryDate, @FreeInquiryLeft, @PaidInquiryLeft,
				 @PackageType, @PackageExpiryDate, @CertificationId,@CertifiedLogoUrl,@IsPremium, @EMI) -- Modified By  Vivek Gupta on 25/11/13  -- Modified By Vivek Gupta on 10-11-2014  


				-- If some data already exist for this Sell Inquiry, delete it considering this action as an update.     
				SET @RecordId = SCOPE_IDENTITY()   
				    

-------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
				INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
												DealerId,
												IsCarUploaded,
												CreatedOn)
								         VALUES(@RecordId,
								                @DealerId,
								                1,
								                GETDATE()
								                )
---------------------------------------------------------------------------------------------------------------------------------------------								           

				DELETE FROM SellInquiriesDetails WHERE SellInquiryId=@RecordId           				

				INSERT INTO SellInquiriesDetails     
				(    
					SellInquiryId,   Owners,   RegistrationPlace,   OneTimeTax,   Insurance,     
					InsuranceExpiry,  UpdateTimeStamp,  InteriorColor, InteriorColorCode, CityMileage,   AdditionalFuel,     
					CarDriven,   Accidental,   FloodAffected,    Warranties,   Modifications, ACCondition,   
					BatteryCondition,  BrakesCondition,  ElectricalsCondition,   EngineCondition,  ExteriorCondition,     
					SeatsCondition,   SuspensionsCondition,  TyresCondition,    OverallCondition , Features_SafetySecurity, Features_Comfort, Features_Others,InteriorCondition,
					YoutubeVideo, IsYouTubeVideoApproved -- Modified By Vivek Gupta on 6th jan 2014
				)    
				VALUES    
				(	    
					@RecordId,   @Owners,   @RegPlace,    @OneTimeTax,   @Insurance,     
					@InsExpiry,   @UpdateTimeStamp,  @InteriorColor, @InteriorColorCode,  @CityMileage,   @AdditionalFuel,     
					@CarDriven,   @Accidental,   @FloodAffected,   @Warranties,   @Modifications,  @AirConditioning,  
					@BatteryCondition,  @BrakesCondition,  @ElectricalsCondition,   @EngineCondition,  @ExteriorCondition,     
					@SeatsCondition,  @SuspensionsCondition, @TyresCondition,   @OverallCondition, @SafetyFeatures, @ComfortFeatures, @OtherFeatures,@InteriorCondition,
					@VideoUrl, @IsYouTubeVideoApproved -- Modified By Vivek Gupta on 6th jan 2014
				)     

				INSERT INTO SellInquiryAccessories(CarId,Accessories,StockCar)    
				VALUES( @RecordId, @Accessories, @StockCar )   
				
				EXEC DCRM_CheckORCars -1, NULL, @RecordId, @Kilometers, @Price, @MakeYear, NULL
			END    
		ELSE -- Updation    
			BEGIN   
			   
			   DECLARE @LastUpdatedDate DATETIME
			   
			   select @OldPrice=Price,@OldCommentLength=LEN(Comments),@LastUpdatedDate=LastUpdated 
			   from SellInquiries WITH (NOLOCK) WHERE ID = @Id -------line add by manish for implementation of logic of lastupdated date for used car.			   
			   
			    ---if condition added by Manish(AE1665) on 26 nov 2012 for last updated date change logic--------
				if ((Abs(@OldPrice- @price)) > ( @OldPrice * .01 )) OR ((LEN(ISNULL(@Comments,0))-ISNULL(@OldCommentLength,0)) > 10) 
				BEGIN
					--UPDATE SellInquiries SET LastUpdated=GETDATE() WHERE ID = @Id
					SET @LastUpdatedDate = GETDATE()
				END		
				
				DECLARE @CertifiedLogoUrlUpdate varchar(200)
				IF(@CertificationId IS NOT NULL)
				BEGIN		
					SELECT @CertifiedLogoUrlUpdate=HostURL + DirectoryPath + LogoURL FROM Classified_CertifiedOrg WITH (NOLOCK) WHERE Id=@CertificationId
				END		
			   
			   -- ModifiedBy:	Surendra on 23 Nov 2012 at 3pm Description: Package expiry date update if stock is already there in Sellinquiries 
			   UPDATE SellInquiries SET DealerId=@DealerId, CarVersionId=@CarVersionId,     
			   CarRegNo=@CarRegNo, ModifiedDate=@EntryDate, StatusId=@StatusId,    
			   Price=@Price, MakeYear=@MakeYear, Kilometers=@Kilometers,Color=@Color,ColorCode=@ColorCode, Comments=@Comments,
			   CertificationId = @CertificationId,PackageExpiryDate=@PackageExpiryDate, LastUpdated = @LastUpdatedDate,
			   CertifiedLogoUrl=@CertifiedLogoUrlUpdate, PackageType=@PackageType,-- Modified By:  Tejashree Patil on 7 Aug 2013,
			   IsPremium = @IsPremium, EMI = @EMI -- Modified By  Vivek Gupta on 25/11/13  -- Modified By Vivek Gupta on 10-11-2014
			   WHERE ID = @Id
    
    		-------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
							INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
															DealerId,
															IsCarUploaded,
															CreatedOn)
													 VALUES(@Id,
															@DealerId,
															1,
															GETDATE()
															)
			---------------------------------------------------------------------------------------------------------------------------------------------	                 
						  
			   
			   --Also update the last updated date all the available cars    
			  -- Update SellInquiries Set LastUpdated=@EntryDate Where DealerId = @DealerId AND ID IN     
				--(Select InquiryId From LiveListings Where SellerType = 1)  
       
			   DELETE FROM SellInquiriesDetails WHERE SellInquiryId=@Id                 

			   INSERT INTO SellInquiriesDetails     
			   (    
				SellInquiryId,   Owners,   RegistrationPlace,   OneTimeTax,   Insurance,     
				InsuranceExpiry,  UpdateTimeStamp,  InteriorColor, InteriorColorCode,    CityMileage,   AdditionalFuel,     
				CarDriven,   Accidental,   FloodAffected,    Warranties,   Modifications, ACCondition,  
				BatteryCondition,  BrakesCondition,  ElectricalsCondition,   EngineCondition,  ExteriorCondition,     
				SeatsCondition,   SuspensionsCondition,  TyresCondition,    OverallCondition, Features_SafetySecurity, Features_Comfort, Features_Others,InteriorCondition,
				YoutubeVideo, IsYouTubeVideoApproved -- Modified By Vivek Gupta on 6th jan 2014
			   )    
			   VALUES    
			   (    
				@Id,    @Owners,   @RegPlace,    @OneTimeTax,   @Insurance,     
				@InsExpiry,   @UpdateTimeStamp,  @InteriorColor, @InteriorColorCode, @CityMileage,   @AdditionalFuel,     
				@CarDriven,   @Accidental,   @FloodAffected,   @Warranties,   @Modifications, @AirConditioning,  
				@BatteryCondition,  @BrakesCondition,  @ElectricalsCondition,   @EngineCondition,  @ExteriorCondition,     
				@SeatsCondition,  @SuspensionsCondition, @TyresCondition,   @OverallCondition, @SafetyFeatures, @ComfortFeatures, @OtherFeatures,@InteriorCondition,
				@VideoUrl, @IsYouTubeVideoApproved -- Modified By Vivek Gupta on 6th jan 2014
			   )              

				DELETE FROM SellInquiryAccessories WHERE CarId=@Id AND StockCar=@StockCar       
    
				INSERT INTO SellInquiryAccessories(CarId,Accessories,StockCar)    
				VALUES( @Id, @Accessories, @StockCar )    
    
				SET @RecordId = @Id  
				
				EXEC DCRM_CheckORCars -1, NULL, @RecordId, @Kilometers, @Price, @MakeYear, NULL  
			END    
     
     
END

/****** Object:  Trigger [dbo].[TrigUpdateDealerCarData]    Script Date: 11/12/2014 12:26:48 ******/
SET ANSI_NULLS ON
