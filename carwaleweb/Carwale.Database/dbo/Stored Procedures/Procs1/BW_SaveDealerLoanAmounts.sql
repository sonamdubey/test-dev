IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerLoanAmounts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerLoanAmounts]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to save the EMI for the given dealer id

--Modified By : Suresh Prajapati on 02nd Dec 2014
--Description : Added "LoanProvider" To save Loan Provider's Name.
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerLoanAmounts]
	-- Add the parameters for the stored procedure here
	@DealerId INT
	,@Tenure TINYINT
	,@RateOfInterest VARCHAR(20)
	,@LTV TINYINT
	,@LoanProvider VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO BW_DealerLoanAmounts (
		DealerId
		,LTV
		,RateOfInterest
		,Tenure
		,LoanProvider
		)
	VALUES (
		@DealerId
		,@LTV
		,@RateOfInterest
		,@Tenure
		,@LoanProvider
		)
END

