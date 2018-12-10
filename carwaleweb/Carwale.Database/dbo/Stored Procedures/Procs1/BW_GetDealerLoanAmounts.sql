IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerLoanAmounts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerLoanAmounts]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to get the EMI for the given dealer id
--Modified By : Suresh Prajapati on 02nd Dec 2014.
--Description : Added "LoanProvider" column To get Loan Provider.  
--
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerLoanAmounts]
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
	FROM BW_DealerLoanAmounts WITH (NOLOCK)
	WHERE DealerId = @DealerId
END

