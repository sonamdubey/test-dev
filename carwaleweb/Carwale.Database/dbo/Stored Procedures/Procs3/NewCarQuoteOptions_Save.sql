IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarQuoteOptions_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarQuoteOptions_Save]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 16-Sep-08 5:48 PM
-- Description:	SP to save finance options for quotations
-- =============================================
CREATE PROCEDURE [dbo].[NewCarQuoteOptions_Save] 
	-- Add the parameters for the stored procedure here
	@QuoteId				Numeric,
	@FaId					Int,
	@ExShowroomPrice		Int,
	@FinanceAmount			Int,
	@MarginAmount			Int,
	@Tenure					Int,
	@LTV					Decimal,
	@FinanceOption			TinyInt,
	@EMI					Int,
	@StampDuty				Int,
	@ProcessingFees			Int,
	@CwDiscount				Int,
	@OtherChargesName		VarChar(50),
	@OtherCharges			Decimal(18,2),
	@TotalDownPayment		Int,
	@IRType					SmallInt,
	@EffectiveInterestRate	Decimal(18,2),
	@CwDealerCommission		Decimal(18,2),
	@EntryDateTime			DateTime,
	@OptionId				Numeric Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert Into NewCarQuoteOptions
	(
		QuoteId, FaId, ExShowroomPrice, FinanceAmount, MarginAmount, 
		Tenure, FinanceOption, EMI, StampDuty, ProcessingFees,
		CwDiscount, OtherChargesName, OtherCharges, TotalDownPayment,
		EffectiveInterestRate,InterestRateType, EntryDateTime, LTV, CwDealerCommission
	) 
	Values
	(
		@QuoteId, @FaId, @ExShowroomPrice, @FinanceAmount, @MarginAmount, 
		@Tenure, @FinanceOption, @EMI, @StampDuty, @ProcessingFees, 
		@CwDiscount, @OtherChargesName, @OtherCharges, @TotalDownPayment, 
		@EffectiveInterestRate, @IRType, @EntryDateTime, @LTV, @CwDealerCommission
	)

	Set @OptionId = Scope_Identity()	
END










