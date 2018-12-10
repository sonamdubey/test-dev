IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_FinalQuote_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_FinalQuote_Save]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 16-Sep-08 5:48 PM
-- Description:	SP to save final price quote of module NCS(New Car Sales)
-- =============================================
CREATE PROCEDURE [dbo].[NCS_FinalQuote_Save] 
	-- Add the parameters for the stored procedure here
	-- Input Parameters
	@PQRefNo			VarChar(50),
	@FAId				Int,
	@CustomerName		VarChar(50),
	@Email				VarChar(100),
	@Mobile				VarChar(12),
	@Comments			VarChar(500),
	@ExShowroomPrice	Int,
	@FinanceAmount		Int,
	@MarginAmount		Int,
	@FinanceTenure		Int,
	@FinanceOption		TinyInt,
	@EMI				Int,
	@StampDuty			Int,
	@ProcessingFees		Int,
	@CarwaleDiscount	Int,
	@InsuranceAmount	Int,
	@RTO				Int,
	@OtherChargesName	VarChar(50),
	@OtherCharges		Int,
	@TotalDownPayment	Int,
	@EffectiveInterestRate Decimal(18,2),
	@CwDealerCommission		Decimal(18,2),
	
	-- Output Parameter
	@QuoteId			Numeric Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert statements for procedure here
	Insert Into Ncs_FinalQuote
	(
		PQRefNo, FAId, CustomerName, Email, Mobile, Comments, ExShowroomPrice, FinanceAmount, MarginAmount, 
		FinanceTenure, FinanceOption, EMI, StampDuty, ProcessingFees,
		CarwaleDiscount, InsuranceAmount, RTO, OtherChargesName, OtherCharges, TotalDownPayment,
		EffectiveInterestRate, CwDealerCommission
	) 
	Values
	(
		@PQRefNo, @FAId, @CustomerName, @Email, @Mobile, @Comments, @ExShowroomPrice, @FinanceAmount, @MarginAmount, 
		@FinanceTenure, @FinanceOption, @EMI, @StampDuty, @ProcessingFees, 
		@CarwaleDiscount, @InsuranceAmount,@RTO, @OtherChargesName, @OtherCharges, @TotalDownPayment, 
		@EffectiveInterestRate, @CwDealerCommission
	)
	
	Set @QuoteId = Scope_Identity()
END








