IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomerSellInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomerSellInquiry]
GO

	


--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE    PROCEDURE [dbo].[InsertCustomerSellInquiry]
	@SellInquiryId		NUMERIC,	-- Sell Inquiry Id
	@CustomerId		NUMERIC,	-- customer ID
	@CarVersionId		NUMERIC,	-- Car Version Id
	@RequestDateTime	DATETIME,	-- Entry Date
	@MakeYear		DATETIME,
	@RegNo		VARCHAR(50),
	@Kms			NUMERIC,
	@Price			NUMERIC,
	@Color			VARCHAR(50),	
	@Comments		VARCHAR(500),
	@SendDealers		BIT,
	@ListInClassifieds	BIT,
	@IsApproved		BIT,
	
	@RegPlace		VARCHAR(50),
	@Owners		NUMERIC,
	@Tax			VARCHAR(50),
	@Insurance		VARCHAR(50),
	@InsuranceExpiry	DATETIME,
	@InteriorColor		VARCHAR(50),
	@Mileage		VARCHAR(50),
	@Fuel			VARCHAR(50),	
	@Driven		VARCHAR(50),
	@Accidental		BIT,
	@FloodAffected		BIT,
	@Accessories 		VARCHAR(500),
	@Warranties		VARCHAR(500),
	@Modifications		VARCHAR(500),
-- vehicle condition --
	@Brakes		VARCHAR(50),
	@Battery		VARCHAR(50),
	@Electricals		VARCHAR(50),
	@Engine		VARCHAR(50),
	@Exterior		VARCHAR(50),
	@Seats			VARCHAR(50),
	@Suspensions 		VARCHAR(50),
	@Tyres			VARCHAR(50),
	@Overall		VARCHAR(50),
	@ID			NUMERIC OUTPUT

 AS
	DECLARE 
		@InquiryId 		NUMERIC,
		@CityId 		NUMERIC,
		@LastBidDate		DATETIME,
		@ClassifiedExpiry	DATETIME,
		@FreeInquiryLeft	INT,
		@PaidInquiryLeft	INT
	BEGIN
	IF @SellInquiryId = -1
	BEGIN	

		--fetch the max paid inquiry and the max free inquiry from the AllowedInquiriesMaster table for the  ConsumerType = 2 for the customers
		SELECT 
			@FreeInquiryLeft = MaxFreeInquiry, @PaidInquiryLeft = MaxPaidInquiry 
		FROM 
			AllowedInquiriesMaster 
		WHERE 
			ConsumerType = 2
				

		SELECT @CityId=CityId FROM Customers WHERE ID=@CustomerId
		
		/*If @CityId=1 
		BEGIN
			SET @LastBidDate 	= DATEADD(DAY,7,@RequestDateTime)
			SET @ClassifiedExpiry 	= DATEADD(DAY,45,@RequestDateTime)
		END		
		ELSE
		BEGIN*/
			SET @ClassifiedExpiry 	= DATEADD(DAY,30,@RequestDateTime)		
		--END

		INSERT INTO CustomerSellInquiries( CustomerId, CarVersionId, CarRegNo, EntryDate, Price,
			MakeYear, Kilometers,Color, Comments, ForwardDealers, ListInClassifieds,IsApproved,
			LastBidDate, ClassifiedExpiryDate, PaidInqLeft, FreeInqLeft, PackageType) 
		VALUES(@CustomerId, @CarVersionId, @RegNo, @RequestDateTime, @Price,
			@MakeYear, @Kms, @Color, @Comments, @SendDealers, @ListInClassifieds,
			@IsApproved, @LastBidDate, @ClassifiedExpiry, @PaidInquiryLeft, @FreeInquiryLeft, 1) 

		SET @InquiryId = SCOPE_IDENTITY()
		SET @ID = @InquiryId

		INSERT INTO CustomerSellInquiryDetails(InquiryId, RegistrationPlace, Insurance, 
			InsuranceExpiry, Owners, Tax, InteriorColor, CityMileage, AdditionalFuel, 
			CarDriven, Accidental, FloodAffected, Accessories, Warranties, Modifications, 
			BatteryCondition, BrakesCondition, ElectricalsCondition, EngineCondition, 
			ExteriorCondition, SeatsCondition, SuspensionsCondition, TyresCondition, OverallCondition)
		VALUES (@InquiryId, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, 
			@Tax, @InteriorColor, @Mileage, @Fuel, @Driven, @Accidental, 
			@FloodAffected, @Accessories, @Warranties, @Modifications, @Battery, 
			@Brakes, @Electricals, @Engine, @Exterior, 
			@Seats, @Suspensions, @Tyres, @Overall)
	END
	ELSE
	BEGIN
		SELECT @ClassifiedExpiry=ClassifiedExpiryDate FROM CustomerSellInquiries 
		WHERE Id=@SellInquiryId
		
		
		-- Check if the inquiry is in bidding ?
		If DATEDIFF(d, @RequestDateTime, @ClassifiedExpiry) < 30 OR @ClassifiedExpiry IS NULL
		BEGIN
			SET @ClassifiedExpiry 	= DATEADD(DAY,30,@RequestDateTime)		
		END
		/**/
		UPDATE CustomerSellInquiries SET CarVersionId=@CarVersionId, 
			CarRegNo=@RegNo, Price=@Price, MakeYear=@MakeYear, Kilometers=@Kms,
			Color=@Color, Comments=@Comments, ClassifiedExpiryDate=@ClassifiedExpiry
		WHERE ID=@SellInquiryId	
		

		DELETE FROM CustomerSellInquiryDetails WHERE InquiryId=@SellInquiryId
		
		INSERT INTO CustomerSellInquiryDetails(InquiryId, RegistrationPlace, Insurance, 
			InsuranceExpiry, Owners, Tax, InteriorColor, CityMileage, AdditionalFuel, 
			CarDriven, Accidental, FloodAffected, Accessories, Warranties, Modifications, 
			BatteryCondition, BrakesCondition, ElectricalsCondition, EngineCondition, 
			ExteriorCondition, SeatsCondition, SuspensionsCondition, TyresCondition, OverallCondition)
		VALUES (@SellInquiryId, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, 
			@Tax, @InteriorColor, @Mileage, @Fuel, @Driven, @Accidental, 
			@FloodAffected, @Accessories, @Warranties, @Modifications, @Battery, 
			@Brakes, @Electricals, @Engine, @Exterior, 
			@Seats, @Suspensions, @Tyres, @Overall)
		SET @Id=@SellInquiryId
	END		
	END