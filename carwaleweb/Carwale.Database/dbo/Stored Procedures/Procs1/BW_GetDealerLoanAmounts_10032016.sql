IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerLoanAmounts_10032016]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerLoanAmounts_10032016]
GO

	-- =============================================
--	Author		:	Ashish G. Kamble
--	Create date	:	31 Oct 2014
--	Description	:	Proc to get the EMI for the given dealer id
--	Modified By :	Suresh Prajapati on 02nd Dec 2014.
--	Description :	Added "LoanProvider" column To get Loan Provider.  
--	Modified By	:	Sumit Kate on 10 Mar 2016
--	Description	:	Retrieve newly Added new columns data
--					Down Payment(min-max)
--					Tenure(min-max)
--					Rate of Interest(min-max)
--					Processing Fee
--	Updated by	: Sangram Nandkhile on 14 March 2016
--	Desc		: Added 2 new columns - MinLtv & maxLtv
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerLoanAmounts_10032016]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT LTV
		,RateOfInterest
		,Tenure
		,LoanProvider
		,MinDownPayment
		,MaxDownPayment
		,MinTenure
		,MaxTenure
		,MinRateOfInterest
		,MaxRateOfInterest
		,MinLtv
		,MaxLtv
		,ProcessingFee
		,ID
	FROM BW_DealerLoanAmounts WITH (NOLOCK)
	WHERE DealerId = @DealerId AND IsActive = 1
END