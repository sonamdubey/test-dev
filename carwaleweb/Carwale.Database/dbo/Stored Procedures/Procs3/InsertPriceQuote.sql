IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPriceQuote]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPriceQuote]
GO

	--THIS PROCEDURE IS FOR INSERTING GENERATED NEW CAR PRICE QUOTES

CREATE PROCEDURE [InsertPriceQuote]
	@QuoteId		NUMERIC OUTPUT,
	@InquiryId		NUMERIC,	-- InquiryId
	@CustomerId		NUMERIC,	-- CustomerID
	@CarVersionId		NUMERIC,	-- Car Version Id
	@CarName		VARCHAR(50),	-- Car Name
	@IsCorporate		BIT,		-- Is it Corporate Quote
	@Price			NUMERIC,	-- Car Price
	@SubCityId		NUMERIC,	-- SubCityId
	@SubCityName		VARCHAR(50),	-- SubCityName
	@Tenure		NUMERIC,	-- Loan is for how many months?
	@FinanceAmount	NUMERIC,	-- What is the loan amount?
	@Margin		NUMERIC, 	-- Price - FinanceAmount
	@EMI			NUMERIC,	-- Calculated Loan EMI
	@AdvanceEMi		NUMERIC,	-- Normally 1. How many EMIs customer wishes to pay in advance
	@Insurance		NUMERIC,	-- Insurance Charges
	@RTO			NUMERIC,	-- Registration Charges
	@TotalDownpayment	NUMERIC,	-- Margin + Insurance + 1 EMI + RTO
	@Discount		NUMERIC,	-- Cash Discounts
	@FinalDownpayment	NUMERIC,	-- TotalDownpayment - Discount
	@OtherDiscounts	VARCHAR(2000	),-- All other Non-cash discounts	
	@DocumentsNeeded	VARCHAR(1000),-- Documents Needed for Finance
	@GeneratedDate	DATETIME	-- Quotation Generation Date	
 AS
	
BEGIN
	INSERT INTO 
		NewCarQuotations
			(
				InquiryId, 	CustomerId,		CarVersionId,		CarName,		IsCorporate,		Price,			SubCityId,
	 			SubCityName,	Tenure,			FinanceAmount,		Margin,			EMI,			AdvanceEMi,		Insurance,
				RTO,		TotalDownpayment,	Discount,		FinalDownpayment,	OtherDiscounts,		DocumentsNeeded,	GeneratedDate
			) 
		VALUES
			(
				@InquiryId,		@CustomerId,		@CarVersionId,		@CarName,		@IsCorporate,@Price,	@SubCityId,		@SubCityName,
				@Tenure,		@FinanceAmount,	@Margin,		@EMI,			@AdvanceEMi,		@Insurance,		@RTO,
				@TotalDownpayment,	@Discount,		@FinalDownpayment,	@OtherDiscounts,	@DocumentsNeeded,	@GeneratedDate
			) 

	SET @QuoteId = SCOPE_IDENTITY()
END