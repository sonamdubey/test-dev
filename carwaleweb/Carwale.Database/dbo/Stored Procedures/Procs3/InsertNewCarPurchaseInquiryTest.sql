IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarPurchaseInquiryTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarPurchaseInquiryTest]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE

Create PROCEDURE [dbo].[InsertNewCarPurchaseInquiryTest]
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId		NUMERIC,	-- Car Version Id
	@CustomerId		NUMERIC,	-- Car RegistrationNo 
	@RequestDateTime	DATETIME,	-- Entry Date
	@Color			VARCHAR(50),
	@NoOfCars		INT,		-- How Many cars customer intend to buy
	@BuyTime		VARCHAR(100),	-- When customer intend to buy? i time-frame
	@Comments 		VARCHAR(2000),
	@IsApproved		BIT,
	@TestdriveDate		VARCHAR(50),
	@TestDriveLocation	VARCHAR(300),
	@LatestOffers		BIT,
	@ForwardedLead	BIT,
	@SourceId		SMALLINT,

	-- Parameters For NewPurchaseCities
	@Name			Varchar(50),
	@CityId			Numeric,
	@City			VARCHAR(50),
	@EmailId		VarChar(100),
	@PhoneNo		VarChar(50),

	-- Output Parameter
	@InquiryId		NUMERIC OUTPUT,
	@QuoteId		NUMERIC OUTPUT

 AS
	BEGIN
		INSERT INTO NewCarPurchaseInquiries
			(	
				CustomerId, 		CarVersionId, 		Color, 			Comments, 
				NoOfCars, 		BuyTime, 		RequestDateTime,	IsApproved,
				TestdriveDate, 		LatestOffers, 		ForwardedLead, 	SourceId,
				TestDriveLocation
			)
		VALUES
			( 	
				@CustomerId, 		@CarVersionId, 		@Color, 		@Comments, 
				@NoOfCars, 		@BuyTime, 		@RequestDateTime,	@IsApproved,
				@TestdriveDate, 	@LatestOffers, 		@ForwardedLead, 	@SourceId,
				@TestDriveLocation
			)

		SET @InquiryId = SCOPE_IDENTITY()
		
		If( @InquiryId > 0 )
		BEGIN
			INSERT INTO NewPurchaseCities( InquiryId, CityId, City, EmailId, PhoneNo, Name )  
			VALUES(@InquiryId, @CityId, @City, @EmailId, @PhoneNo, @Name)

			Declare	@ExShowroomPrice Numeric, @RTO Numeric, @Insurance Numeric

			SELECT @ExShowroomPrice = Price, @RTO = IsNull(RTO, 0), @Insurance = IsNull(Insurance, 0)
			FROM NewCarShowroomPrices
			WHERE CarVersionId = @CarVersionId AND CityId = @CityId
				
			IF( @ExShowroomPrice > 0 )
			BEGIN
				INSERT INTO NewCarQuotes(NewCarInqId, ExShowroomPrice, RTO, Insurance )
				Values(@InquiryId, @ExShowroomPrice, @RTO, @Insurance)

				Set @QuoteId = SCOPE_IDENTITY()
			END
		END
	END




