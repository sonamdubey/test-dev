IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCustomerSellInquiriesEditRequest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCustomerSellInquiriesEditRequest]
GO
	

--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

CREATE    PROCEDURE [dbo].[InsertCustomerSellInquiriesEditRequest]
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
	@Overall		VARCHAR(50)

AS

	BEGIN	
		INSERT INTO CustomerSellInquiriesEditRequest(InquiryId, CustomerId, CarVersionId, CarRegNo, EntryDate, Price,
			MakeYear, Kilometers,Color, Comments, RegistrationPlace, Insurance, 
			InsuranceExpiry, Owners, Tax, InteriorColor, CityMileage, AdditionalFuel, 
			CarDriven, Accidental, FloodAffected, Accessories, Warranties, Modifications, 
			BatteryCondition, BrakesCondition, ElectricalsCondition, EngineCondition, 
			ExteriorCondition, SeatsCondition, SuspensionsCondition, TyresCondition, OverallCondition) 
		
		VALUES(@SellInquiryId, @CustomerId, @CarVersionId, @RegNo, @RequestDateTime, @Price,
			@MakeYear, @Kms, @Color, @Comments, @RegPlace, @Insurance, @InsuranceExpiry, @Owners, 
			@Tax, @InteriorColor, @Mileage, @Fuel, @Driven, @Accidental, 
			@FloodAffected, @Accessories, @Warranties, @Modifications, @Battery, 
			@Brakes, @Electricals, @Engine, @Exterior, 
			@Seats, @Suspensions, @Tyres, @Overall) 

		UPDATE CustomerSellInquiries Set CarVersionId = @CarVersionId, Price = @Price, Kilometers = @Kms WHERE Id = @SellInquiryId
		
	END