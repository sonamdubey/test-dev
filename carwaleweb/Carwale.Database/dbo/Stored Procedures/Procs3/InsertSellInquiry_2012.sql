IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertSellInquiry_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertSellInquiry_2012]
GO

	

-- =============================================
-- ModifiedBy:	Surendra
-- Create date: 15 may 2012
-- Description:	last updated date will changed only in case where ip price is greater then exisiting price
-- ModifiedBy:	Tejashree Patil on 8 Nov 2012 at 4pm Description: @Color VARCHAR(50) from VARCHAR(30)
-- ModifiedBy:	Surendra on 23 Nov 2012 at 3pm Description: Package expiry date update if stock is already there in Sellinquiries
-- =============================================    
CREATE PROCEDURE [dbo].[InsertSellInquiry_2012]    
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
	 @InteriorCondition  VARCHAR(50)=NULL
 AS    
	 DECLARE     
	  @FreeInquiryLeft INT,    
	  @PaidInquiryLeft INT    
BEGIN      
	IF @Id = -1 AND @ImportChecksum <> -1 -- Maybe Update or Insert. Will have to check    
		BEGIN    
			SELECT @Id=ID FROM SellInquiries WHERE DealerId=@DealerId AND ImportChecksum=@ImportChecksum      
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
				FROM AllowedInquiriesMaster 
				WHERE ConsumerType = 1    
			    
				INSERT INTO SellInquiries( DealerId, CarVersionId, CarRegNo, EntryDate, StatusId, Price,    
				MakeYear, Kilometers,Color,ColorCode, Comments, ImportChecksum, LastUpdated, FreeInqLeft, PaidInqLeft, PackageType, PackageExpiryDate, CertificationId)     
				VALUES(@DealerId, @CarVersionId, @CarRegNo, @EntryDate, @StatusId, @Price,    
				@MakeYear, @Kilometers, @Color, @ColorCode, @Comments, @ImportChecksum, @EntryDate, @FreeInquiryLeft, @PaidInquiryLeft, @PackageType, @PackageExpiryDate, @CertificationId)    


				-- If some data already exist for this Sell Inquiry, delete it considering this action as an update.     
				SET @RecordId = SCOPE_IDENTITY()       

				DELETE FROM SellInquiriesDetails WHERE SellInquiryId=@RecordId           

				INSERT INTO SellInquiriesDetails     
				(    
					SellInquiryId,   Owners,   RegistrationPlace,   OneTimeTax,   Insurance,     
					InsuranceExpiry,  UpdateTimeStamp,  InteriorColor, InteriorColorCode, CityMileage,   AdditionalFuel,     
					CarDriven,   Accidental,   FloodAffected,    Warranties,   Modifications, ACCondition,   
					BatteryCondition,  BrakesCondition,  ElectricalsCondition,   EngineCondition,  ExteriorCondition,     
					SeatsCondition,   SuspensionsCondition,  TyresCondition,    OverallCondition , Features_SafetySecurity, Features_Comfort, Features_Others,InteriorCondition
				)    
				VALUES    
				(	    
					@RecordId,   @Owners,   @RegPlace,    @OneTimeTax,   @Insurance,     
					@InsExpiry,   @UpdateTimeStamp,  @InteriorColor, @InteriorColorCode,  @CityMileage,   @AdditionalFuel,     
					@CarDriven,   @Accidental,   @FloodAffected,   @Warranties,   @Modifications,  @AirConditioning,  
					@BatteryCondition,  @BrakesCondition,  @ElectricalsCondition,   @EngineCondition,  @ExteriorCondition,     
					@SeatsCondition,  @SuspensionsCondition, @TyresCondition,   @OverallCondition, @SafetyFeatures, @ComfortFeatures, @OtherFeatures,@InteriorCondition
				)     
			
				INSERT INTO SellInquiryAccessories(CarId,Accessories,StockCar)    
				VALUES( @RecordId, @Accessories, @StockCar )   
				
				EXEC DCRM_CheckORCars -1, NULL, @RecordId, @Kilometers, @Price, @MakeYear, NULL
			END    
		ELSE -- Updation    
			BEGIN   
			   -- ModifiedBy:	Surendra on 23 Nov 2012 at 3pm Description: Package expiry date update if stock is already there in Sellinquiries 
			   UPDATE SellInquiries SET DealerId=@DealerId, CarVersionId=@CarVersionId,     
			   CarRegNo=@CarRegNo, ModifiedDate=@EntryDate, StatusId=@StatusId,    
			   Price=@Price, MakeYear=@MakeYear, Kilometers=@Kilometers,Color=@Color,ColorCode=@ColorCode, Comments=@Comments,
			   CertificationId = @CertificationId,PackageExpiryDate=@PackageExpiryDate
			   WHERE ID = @Id
    
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
				SeatsCondition,   SuspensionsCondition,  TyresCondition,    OverallCondition, Features_SafetySecurity, Features_Comfort, Features_Others,InteriorCondition
			   )    
			   VALUES    
			   (    
				@Id,    @Owners,   @RegPlace,    @OneTimeTax,   @Insurance,     
				@InsExpiry,   @UpdateTimeStamp,  @InteriorColor, @InteriorColorCode, @CityMileage,   @AdditionalFuel,     
				@CarDriven,   @Accidental,   @FloodAffected,   @Warranties,   @Modifications, @AirConditioning,  
				@BatteryCondition,  @BrakesCondition,  @ElectricalsCondition,   @EngineCondition,  @ExteriorCondition,     
				@SeatsCondition,  @SuspensionsCondition, @TyresCondition,   @OverallCondition, @SafetyFeatures, @ComfortFeatures, @OtherFeatures,@InteriorCondition
			   )     
       
				DELETE FROM SellInquiryAccessories WHERE CarId=@Id AND StockCar=@StockCar       
    
				INSERT INTO SellInquiryAccessories(CarId,Accessories,StockCar)    
				VALUES( @Id, @Accessories, @StockCar )    
    
				SET @RecordId = @Id  
				
				EXEC DCRM_CheckORCars -1, NULL, @RecordId, @Kilometers, @Price, @MakeYear, NULL  
			END    
     
     
END    
    
    
 

