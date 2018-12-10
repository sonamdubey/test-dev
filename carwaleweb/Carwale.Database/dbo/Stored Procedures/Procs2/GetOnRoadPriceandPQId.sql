IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOnRoadPriceandPQId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOnRoadPriceandPQId]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <16/4/2013>
-- Description:	<Description:We fetch InquiryId and On-RoadPrice using this Sp>
-- Modified By : Raghu on <27/12/2013> Added WITH(NOLOCK) and removed insertion into NewCarQuotes Table
-- Modified By : Raghu on <14/1/2014> Added @CustomerId as parameter and removed EXEC GetCustomerId Sp to get CustomerId
-- Modified By : Raghu on <2/6/2014> Added @ForwardedLead Parameter
-- Modified By : Raghu on <4/8/2014> Removed @CustomerId as parameter
-- =============================================
CREATE PROCEDURE [dbo].[GetOnRoadPriceandPQId]
	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarPurchaseInquiries
	@CarVersionId	NUMERIC(18,0),	-- Car Version Id
	@BuyTime		VARCHAR(100),	-- When customer intend to buy? i time-frame

	-- Parameters For NewPurchaseCities
	@Name			Varchar(50),
	@CityId			Numeric(18,0),
	@EmailId		VarChar(100),
	@PhoneNo		VarChar(50),
	@ForwardedLead  BIT = 1,
	@SourceId		TINYINT,
	@PQPageId       SMALLINT = -1,
	@ClientIP       Varchar(100),
	@InterestedInLoan BIT = 0, -- Added by Raghu to capture user's interest to apply for loan
	@MobVerified	 BIT = 0, -- Added by Raght to cpature whether customer verified his mobile or not
	@ZoneId			INT,
	--@CustomerId		NUMERIC(18,0) = null,
	-- OUTPUT PARAMETERS
	@InquiryId NUMERIC OUTPUT,
	@QuoteId  NUMERIC = 0 OUTPUT

 AS
	BEGIN
	    --DECLARE @CustomerId NUMERIC
	    DECLARE @City VARCHAR(50)
	    --Fetch CustomerId if present otherwise register customer and then fetch
	    --EXEC GetCustomerId @Name,@CityId,@EmailId,@PhoneNo,@CustomerId OUTPUT
	    
	    SELECT @City=Name FROM Cities WHERE ID=@CityId

		INSERT INTO NewCarPurchaseInquiries
			(	
				CustomerId, 		
				CarVersionId, 		
				BuyTime, 		RequestDateTime ,IsApproved,LatestOffers,ForwardedLead, -- Added by Raghu
				SourceId,PQPageId,ClientIP
			)
		VALUES
			( 	
				-1,  -----Taking hardcoded value will be updated later after processing in rabbit MQ 		 		
				@CarVersionId ,		 
				@BuyTime, 		GETDATE(),1,1,@ForwardedLead,
				@SourceId,@PQPageId,@ClientIP
			)

		SET @InquiryId = SCOPE_IDENTITY()
	
		IF( @InquiryId > 0 )
		BEGIN
			
			SELECT PQC.CategoryId, Ci.Id AS CategoryItemId, CI.CategoryName, PQN.PQ_CategoryItemValue AS Value,PQLT.IsTaxOnTax
			FROM CW_NewCarShowroomPrices PQN WITH(NOLOCK)
			INNER JOIN PQ_CategoryItems CI WITH(NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
			INNER JOIN PQ_Category PQC WITH(NOLOCK) ON PQC.CategoryId = CI.CategoryId
			LEFT JOIN PriceQuote_LocalTax PQLT WITH(NOLOCK) ON CI.Id = PQLT.CategoryItemid AND PQLT.CityId = @CityId
			WHERE CarVersionId = @CarVersionId AND PQN.CityId = @CityId
			ORDER BY PQC.SortOrder ASC

			INSERT INTO NewPurchaseCities( InquiryId, CityId, City, EmailId, PhoneNo, Name,InterestedInLoan,MobileVerified,ZoneId)  
			VALUES(@InquiryId, @CityId, @City, @EmailId, @PhoneNo, @Name,@InterestedInLoan,@MobVerified,@ZoneId)
            
				
			Declare	@ExShowroomPrice Numeric, @RTO Numeric, @Insurance Numeric

			SELECT @ExShowroomPrice = Price, @RTO = IsNull(RTO, 0), @Insurance = IsNull(Insurance, 0)
			FROM NewCarShowroomPrices WITH(NOLOCK) -- Added by Raghu
			WHERE CarVersionId = @CarVersionId AND CityId = @CityId AND IsActive = 1

			IF( @ExShowroomPrice > 0 )
			BEGIN
				--INSERT INTO NewCarQuotes(NewCarInqId, ExShowroomPrice, RTO, Insurance )
				--Values(@InquiryId, @ExShowroomPrice, @RTO, @Insurance)
				--SET @QuoteId = SCOPE_IDENTITY()
				SET @QuoteId = 1
			END
					
		END
END





