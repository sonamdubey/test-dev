IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarQuoteCompare_CW_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarQuoteCompare_CW_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 24-Mar-2009 1:47 PM
-- Description:	SP to save quote details of customer taken from external dealer
				--These details will be compared with CarWale quote
-- =============================================
CREATE PROCEDURE [dbo].[NewCarQuoteCompare_CW_SP]
	-- Add the parameters for the stored procedure here
	@DealerQuoteId		NUMERIC,
	@QuoteId			NUMERIC,
	@FaId				INT,
	@OptionType			TINYINT,
	@ExShowroomPrice	NUMERIC,
	@RTO				NUMERIC,
	@Insurance			NUMERIC,
	@Discount			NUMERIC,
	@LTV				DECIMAL(18,2),
	@LoanAmount			NUMERIC,
	@LoanTenure			INT,
	@StampDuty			NUMERIC,
	@ProcessingFees		NUMERIC,
	@EMI				NUMERIC,
	@InterestRate		Decimal(18,2),
	@TotalDownPayment	NUMERIC,
	@Status				BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO NewCarQuoteCompare_CW(
		DealerQuoteId, QuoteId, FaId, OptionType, 
		ExShowroomPrice, RTO, Insurance, Discount, LTV,
		LoanAmount,LoanTenure, StampDuty, ProcessingFees, 
		EMI, InterestRate, TotalDownPayment
	)Values(
		@DealerQuoteId, @QuoteId, @FaId, @OptionType, 
		@ExShowroomPrice, @RTO, @Insurance, @Discount, @LTV,
		@LoanAmount,@LoanTenure, @StampDuty, @ProcessingFees, 
		@EMI, @InterestRate, @TotalDownPayment
	)
	IF SCOPE_IDENTITY() > 0
		SET @Status = 1
	ELSE
		SET @Status = 0
END


