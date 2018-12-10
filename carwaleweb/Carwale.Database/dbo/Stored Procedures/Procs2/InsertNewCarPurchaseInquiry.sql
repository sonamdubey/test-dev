IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewCarPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertNewCarPurchaseInquiry]
GO
	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR SellInquiries TABLE
-- Modifed By Raghu : on 27/11/2013 (Added ClientIP,InterestedinLoan, Mobileverified Columns)
-- Modified By Supriya : on 22/5/2014 (Added Zone column) 

CREATE PROCEDURE [dbo].[InsertNewCarPurchaseInquiry]

	-- Parameters For NewCarPurchaseInquiries

	@CarVersionId		NUMERIC,	-- Car Version Id

	@CustomerId		NUMERIC = -1,	-- Car RegistrationNo 

	@RequestDateTime	DATETIME,	-- Entry Date

	@Color			VARCHAR(50) = NULL,

	@NoOfCars		INT = NULL,		-- How Many cars customer intend to buy

	@BuyTime		VARCHAR(100),	-- When customer intend to buy? i time-frame

	@Comments 		VARCHAR(2000) = NULL,

	@IsApproved		BIT,

	@TestdriveDate		VARCHAR(50),

	@TestDriveLocation	VARCHAR(300),

	@LatestOffers		BIT,

	@ForwardedLead	BIT,

	@SourceId		SMALLINT,

    --@PQPageId       SMALLINT = -1, -- Added by Raghu to track from which page user requested pricequote.

	@InterestedInLoan BIT = 0, -- Added by Raghu to capture user's interest to apply for loan
	@MobVerified	 BIT = 0, -- Added by Raght to cpature whether customer verified his mobile or not

	-- Parameters For NewPurchaseCities

	@Name			Varchar(50),

	@CityId			Numeric,

	@City			VARCHAR(50),

	@EmailId		VarChar(100),

	@PhoneNo		VarChar(50),

	@ClientIP		VarChar(100) = NULL, -- Added by Raghu to capture user's clientip

	@Zone    INT = NULL,

	-- Output Parameter

	@InquiryId		NUMERIC OUTPUT,

	@QuoteId		NUMERIC OUTPUT



 AS
	BEGIN
		INSERT INTO NewCarPurchaseInquiries

			(
				CustomerId, 		CarVersionId, 
				BuyTime, 		RequestDateTime,	IsApproved,
				TestdriveDate, 		LatestOffers, 		ForwardedLead, 	SourceId,
				TestDriveLocation ,ClientIP
			)

		VALUES
			(
				@CustomerId, 		@CarVersionId, 
				@BuyTime, 		@RequestDateTime,	@IsApproved,
				@TestdriveDate, 	@LatestOffers, 		@ForwardedLead, 	@SourceId,
				@TestDriveLocation ,@ClientIP
			)

		SET @InquiryId = SCOPE_IDENTITY()

		If( @InquiryId > 0 )

		BEGIN

			INSERT INTO NewPurchaseCities( InquiryId, CityId, City, EmailId, PhoneNo, Name,InterestedInLoan,MobileVerified,ZoneId)  
			VALUES(@InquiryId, @CityId, @City, @EmailId, @PhoneNo, @Name,@InterestedInLoan,@MobVerified,@Zone)


			DECLARE	@ExShowroomPrice Numeric, @RTO Numeric, @Insurance Numeric

			SELECT @ExShowroomPrice = Price, @RTO = IsNull(RTO, 0), @Insurance = IsNull(Insurance, 0)
			FROM NewCarShowroomPrices WITH (NOLOCK)
			WHERE CarVersionId = @CarVersionId AND CityId = @CityId AND IsActive = 1

			IF( @ExShowroomPrice > 0 )
			BEGIN
				INSERT INTO NewCarQuotes(NewCarInqId, ExShowroomPrice, RTO, Insurance )
				Values(@InquiryId, @ExShowroomPrice, @RTO, @Insurance)
				SET @QuoteId = SCOPE_IDENTITY()
			END

		END

	END


