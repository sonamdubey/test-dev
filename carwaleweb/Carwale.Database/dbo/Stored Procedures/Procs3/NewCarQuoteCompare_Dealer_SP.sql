IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarQuoteCompare_Dealer_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarQuoteCompare_Dealer_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 24-Mar-2009 1:47 PM
-- Description:	SP to save quote details of customer taken from external dealer
				--These details will be compared with CarWale quote
-- =============================================
CREATE PROCEDURE [dbo].[NewCarQuoteCompare_Dealer_SP]
	-- Add the parameters for the stored procedure here
	@VersionId			INT,
	@CityId				INT,
	@ExShowroomPrice	NUMERIC,
	@RTO				INT,
	@Insurance			INT,
	@OEMDiscounts		INT,
	@ExchangeBonus		INT,
	@CorporateDiscount	INT,
	@InsuranceDiscount	INT,
	@OtherDiscounts		INT,
	@TotalDiscount		INT,
	@LoanAmount			INT,
	@LoanTenure			SMALLINT,
	@EMI				INT,
	@TotalDownPayment	NUMERIC,
	@StampDutyProcessingFee INT,
	@DealerQuoteId		NUMERIC OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO NewCarQuoteCompare_Dealer(
		VersionId, CityId, ExShowroomPrice, RTO, Insurance,
		OEMDiscounts, ExchangeBonus, CorporateDiscount,
		InsuranceDiscount, OtherDiscounts,TotalDiscount, LoanAmount,
		LoanTenure, EMI, TotalDownPayment, StampDutyProcessingFee
	)Values(
		@VersionId, @CityId, @ExShowroomPrice, @RTO, @Insurance,
		@OEMDiscounts, @ExchangeBonus, @CorporateDiscount,
		@InsuranceDiscount, @OtherDiscounts, @TotalDiscount, @LoanAmount,
		@LoanTenure, @EMI, @TotalDownPayment, @StampDutyProcessingFee
	)

	SET @DealerQuoteId = SCOPE_IDENTITY()
END

