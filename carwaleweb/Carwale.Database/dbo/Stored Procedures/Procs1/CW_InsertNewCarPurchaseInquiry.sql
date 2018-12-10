IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_InsertNewCarPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_InsertNewCarPurchaseInquiry]
GO

	
-- =============================================
-- Author: Raghu
-- Create date: 29/4/2013
-- Description:	Tracks details of the customers for NewCarPurchase
-- Modified By : Raghu on 30-12-2013 (Added WITH(NOLOCK) Condition
-- Modified By : Raghu on <14/1/2014> Added @CustomerId as parameter and removed EXEC GetCustomerId Sp to get CustomerId
-- Modified By : Raghu on <2/6/2014> Added @ForwardedLead Parameter
-- Modified By : Raghu on <4/8/2014> Removed @CustomerId as parameter
-- =============================================

CREATE PROCEDURE [dbo].[CW_InsertNewCarPurchaseInquiry]
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId		NUMERIC,	-- Car Version Id
	@BuyTime		VARCHAR(100),	-- When customer intend to buy? i time-frame
	@SourceId		SMALLINT,

	-- Parameters For NewPurchaseCities
	@Name			Varchar(50),
	@CityId			Numeric,
	--@City			VARCHAR(50),
	@EmailId		VarChar(100),
	@PhoneNo		VarChar(50),
	@ForwardedLead  BIT =1,
	@PQPageId       SMALLINT = -1,
	@ClientIP       Varchar(100),
	@InterestedInLoan BIT = 0, -- Added by Raghu to capture user's interest to apply for loan
	@MobVerified	 BIT = 0, -- Added by Raght to cpature whether customer verified his mobile or not
	@ZoneId INT,
	--@CustomerId		NUMERIC(18,0),
	-- Output Parameter
	@InquiryId		NUMERIC OUTPUT,
	@QuoteId		NUMERIC = 0 OUTPUT

 AS
	BEGIN
		--DECLARE @CustomerId NUMERIC
		DECLARE @City VARCHAR(50)
	    --Fetch CustomerId if present otherwise register customer and then fetch
	    --EXEC GetCustomerId @Name,@CityId,@EmailId,@PhoneNo,@CustomerId OUTPUT
	    
	    SELECT @City=Name FROM Cities WITH(NOLOCK) WHERE ID=@CityId

		INSERT INTO NewCarPurchaseInquiries
			(	
				CustomerId, 		
				CarVersionId,		
				BuyTime, 		RequestDateTime,	IsApproved,LatestOffers,ForwardedLead, -- Added By Raghu
				SourceId ,PQPageId ,ClientIP
			)
		VALUES
			( 	
				-1,  -----Taking hardcoded value will be updated later after processing in rabbit MQ 		
				@CarVersionId,	
				@BuyTime, 		GETDATE(),	1,1,@ForwardedLead,-- Added by Raghu
				@SourceId , @PQPageId,@ClientIP
			)

		SET @InquiryId = SCOPE_IDENTITY()
		
		If( @InquiryId > 0 )
		BEGIN
			INSERT INTO NewPurchaseCities( InquiryId, CityId, City, EmailId, PhoneNo, Name, InterestedInLoan,MobileVerified,ZoneId)  
			VALUES(@InquiryId, @CityId, @City, @EmailId, @PhoneNo, @Name,@InterestedInLoan,@MobVerified,@ZoneId)

			Declare	@ExShowroomPrice Numeric, @RTO Numeric, @Insurance Numeric

			SELECT @ExShowroomPrice = Price, @RTO = IsNull(RTO, 0), @Insurance = IsNull(Insurance, 0)
			FROM NewCarShowroomPrices WITH(NOLOCK)
			WHERE CarVersionId = @CarVersionId AND CityId = @CityId
				
			IF( @ExShowroomPrice > 0 )
			BEGIN
				SET @QuoteId = 1
			END
		END
	END
	
	



